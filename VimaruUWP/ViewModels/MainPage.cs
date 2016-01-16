using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using VimaruUWP.Models;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Windows.Web.Http;
using _9padv4.Models;

namespace VimaruUWP.ViewModels
{
    public class MainPage : BaseModel
    {
        private string form_build_id;
        private LoadState _traDiemLoadState;

        public string MaSV { get; set; }
        public LoadState LoadState { get; internal set; }
        public ObservableCollection<NamHoc> DSNamHoc { get; set; }
        public string HoTen { get; set; }
        public string NgaySinh { get; set; }
        public string Lop { get; set; }
        public LoadState TraDiemLoadState { get { return _traDiemLoadState; } private set { _traDiemLoadState = value; NotifyPropertyChanged("TraDiemLoadState"); } }

        public string TCTichLuy { get; private set; }
        public string TBCTichLuy { get; private set; }

        public MainPage()
        {
            DSNamHoc = new ObservableCollection<NamHoc>();
        }

        internal async Task LoadData()
        {
            LoadState = LoadState.Loading;
            var data = await Utils.DownloadString("http://khaothi.vimaru.edu.vn/tracuudiem");
            var regex = new Regex(@"form_build_id"" value=""(?<form_build_id>[^""]+)");
            var match = regex.Match(data);
            if (match.Success)
            {
                form_build_id = match.Groups["form_build_id"].ToString();
                LoadState = LoadState.Loaded;
                //Utils.ShowMessage(form_build_id);
            }
            else
                LoadState = LoadState.Empty;
        }

        internal async Task TraDiem()
        {
            if (string.IsNullOrEmpty(MaSV))
            {
                await Utils.ShowMessage("Nhập mã SV!");
                return;
            }
            TraDiemLoadState = LoadState.Loading;
            DSNamHoc.Clear();
            var content = new HttpMultipartFormDataContent();
            content.Add(new HttpStringContent(MaSV), "masv");
            content.Add(new HttpStringContent("tra_diem_truc_tuyen_form"), "form_id");
            content.Add(new HttpStringContent(form_build_id), "form_build_id");
            var data = await Utils.UploadString("http://khaothi.vimaru.edu.vn/system/ajax", null, content);
            var obj = JArray.Parse(data);
            var html = obj[1]["data"].ToString();
            var regex = new Regex(@"Họ tên:\s+<b>(?<hoten>[^<]+).+sinh:\s+<b>(?<ngaysinh>[^<]+).+chính:\s+<b>(?<lop>[^<]+).+lũy:\s+<b>(?<tc>[^<]+).+lũy:\s+<b>(?<tbc>[^<]+)");
            var match = regex.Match(html);
            HoTen = string.Empty;
            if (match.Success)
            {
                HoTen = match.Groups["hoten"].ToString();
                NgaySinh = match.Groups["ngaysinh"].ToString();
                Lop = match.Groups["lop"].ToString();
                TCTichLuy = match.Groups["tc"].ToString();
                TBCTichLuy = match.Groups["tbc"].ToString();
                NotifyPropertyChanged("HoTen");
                NotifyPropertyChanged("NgaySinh");
                NotifyPropertyChanged("Lop");
                NotifyPropertyChanged("TCTichLuy");
                NotifyPropertyChanged("TBCTichLuy");
            }
            else {
                LoadState = LoadState.Empty;
                await Utils.ShowMessage("Không có dữ liệu sv này!");
                return;
            }

            var dsTableNamHoc = html.Split(new string[] { "<table>" }, StringSplitOptions.None);
            var regex1 = new Regex("<strong>(?<ten>[^<]+)");
            var regex2 = new Regex(@"<tr><td[^>]+>\d+</td><td>(?<ten>[^<]*)</td><td>(?<tcht>[^<]*)</td><td>(?<x>[^<]*)</td><td>(?<y>[^<]*)</td><td>(?<z10>[^<]*)</td><td>(?<z4>[^<]*)</td><td>(?<chu>[^<]*)</td>");
            
            for (var i = 1; i < dsTableNamHoc.Length; i++)
            {

                var namhoc = new NamHoc();

                var match1 = regex1.Match(dsTableNamHoc[i]);
                if (match1.Success)
                    namhoc.Ten = match1.Groups["ten"].ToString();
                MatchCollection matches;

                try
                {
                    matches = regex2.Matches(dsTableNamHoc[i]);
                    if (matches.Count > 0)
                    {
                        foreach (Match match2 in matches)
                        {
                            var monhoc = new MonHoc
                            {
                                TenMon = match2.Groups["ten"].ToString(),
                                TCHT = match2.Groups["tcht"].ToString(),
                                X = match2.Groups["x"].ToString(),
                                Y = match2.Groups["y"].ToString(),
                                Z10 = match2.Groups["z10"].ToString(),
                                Z4 = match2.Groups["z4"].ToString(),
                                DiemChu = match2.Groups["chu"].ToString(),
                            };
                            namhoc.DSMonHoc.Add(monhoc);
                        }
                    }
                    DSNamHoc.Add(namhoc);
                }
                catch (Exception ex)
                {
                    await Utils.ShowMessage(ex.Message);
                }
            }
            TraDiemLoadState =  LoadState.Loaded;
        }
    }
}
