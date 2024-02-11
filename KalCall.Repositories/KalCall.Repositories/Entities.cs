using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalCall.Repositories
{
    public class FileData
    {
        public bool? exists { get; set; }
        public string name { get; set; }
        public string uniqueId { get; set; }
        public string fileType { get; set; }
        public double? size { get; set; }
        public DateTime? mtime { get; set; }
        public double? duration { get; set; }
        public FileMeta? meta { get; set; }
    }
    public class FileMeta
    {
        public string title { get; set; }
    }
    public class ExtIni
    {
        public string jcalendar_realdate { get; set; }
        public string up { get; set; }
        public string root { get; set; }
        public string Admin { get; set; }
        public string digits { get; set; }
        public string timeout { get; set; }
        public string start_select_digits { get; set; }
        public string file_amount_digits { get; set; }
        public string attempts { get; set; }
        public string control_play1 { get; set; }
        public string control_play2 { get; set; }
        public string control_play3 { get; set; }
        public string control_play4 { get; set; }
        public string control_play5 { get; set; }
        public string control_play6 { get; set; }
        public string control_play8 { get; set; }

        [JsonProperty("control_play*")]
        public string ControlPlay { get; set; }


        [JsonProperty("control_play_moreA*")]
        public string ControlPlayMoreA { get; set; }
        public string control_play_moreA1 { get; set; }
        public string control_play_moreA2 { get; set; }
        public string control_play_moreA3 { get; set; }
        public string control_play_moreA4 { get; set; }
        public string control_play_moreA5 { get; set; }
        public string control_play_moreA6 { get; set; }
        public string control_play_moreA7 { get; set; }
        public string control_after_play_moreA0 { get; set; }
        public string control_after_play_moreA1 { get; set; }
        public string control_play7 { get; set; }
        public string control_play9 { get; set; }
        public string control_play0 { get; set; }

        public string control_play_moreA0 { get; set; }
        public string control_play_moreA8 { get; set; }
        public string control_play_moreA9 { get; set; }

        [JsonProperty("control_playlist*")]
        public string ControlPlaylist { get; set; }
        public string music_on_hold { get; set; }
        public string language { get; set; }
        public string copy_link { get; set; }
        public string playfile_say_replies { get; set; }
        public string hard_link { get; set; }
        public string type { get; set; }
        public string title { get; set; }
    }

    public class File
    {
        public bool? exists { get; set; }
        public string name { get; set; }
        public string uniqueId { get; set; }
        public string fileType { get; set; }
        public int? size { get; set; }
        public string mtime { get; set; }
        public double? duration { get; set; }
        public string durationStr { get; set; }
        public string customerDid { get; set; }
        public Meta meta { get; set; }
        public string source { get; set; }
        public string date { get; set; }
        public object phone { get; set; }
        public string ip { get; set; }
        public string what { get; set; }
    }
    public class Ini
    {
        public bool? exists { get; set; }
        public string name { get; set; }
        public string uniqueId { get; set; }
        public string fileType { get; set; }
        public int? size { get; set; }
        public string mtime { get; set; }
        public string what { get; set; }
    }
    public class Meta
    {
        public string title { get; set; }
    }
    public class Message
    {
        public bool? exists { get; set; }
        public string name { get; set; }
        public string uniqueId { get; set; }
        public string fileType { get; set; }
        public object size { get; set; }
        public object mtime { get; set; }
        public object duration { get; set; }
        public object durationStr { get; set; }
        public object customerDid { get; set; }
        public object meta { get; set; }
        public object source { get; set; }
        public object date { get; set; }
        public object phone { get; set; }
        public object ip { get; set; }
        public string what { get; set; }
    }

    public class Root
    {
        public string responseStatus { get; set; }
        public ExtIni extIni { get; set; }
        public string thisPath { get; set; }
        public string parentPath { get; set; }
        public List<object> dirs { get; set; }
        public List<File> files { get; set; }
        public List<Ini> ini { get; set; }
        public List<Message> messages { get; set; }
        public List<object> html { get; set; }
        public int? yemotAPIVersion { get; set; }
    }
}
