using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Emzi0767.StoAcademyTools.Library.Data;
using Emzi0767.StoAcademyTools.Library.Data.Attributes;
using Emzi0767.StoAcademyTools.Library.Data.Enums;
using Newtonsoft.Json.Linq;

namespace Emzi0767.StoAcademyTools.Library
{
    /// <summary>
    /// Contains a collection of tools to work with Star Trek Online Academy skill planner data.
    /// </summary>
    public class StoAcademyInterface
    {
        internal const string URL_BASE = "http://skillplanner.stoacademy.com/";
        internal const string URL_DATA = "http://skillplanner.stoacademy.com/data";
        internal const string URL_BUILD = "http://skillplanner.stoacademy.com/build/";
        internal const string URL_ITEM = "http://skillplanner.stoacademy.com/item/created?id=";

        private bool initialized = false;
        private Dictionary<string, DoffSpecialization> doff_spec_dictionary = null;
        private Dictionary<int, StoAcademyItem> item_cache = null;
        private List<Cookie> cookies = null;
        private StoaData data = default(StoaData);

        /// <summary>
        /// Creates a new instance of the STOA toolkit.
        /// </summary>
        public StoAcademyInterface()
        {
            this.initialized = false;
        }

        /// <summary>
        /// Initializes the toolkit.
        /// </summary>
        public void Initialize()
        {
            this.InitDictionaries();

            var utf = new UTF8Encoding(false);

            var uri = new Uri(URL_BASE);
            var req = WebRequest.CreateHttp(uri);
            req.Headers.Clear();
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            req.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.5");
            req.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");
            req.Headers.Add("DNT: 1");
            req.Host = "skillplanner.stoacademy.com";
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:48.0) Gecko/20100101 Firefox/48.0.1 Waterfox/48.0.1";
            req.CookieContainer = new CookieContainer();

            var res = (HttpWebResponse)req.GetResponse();
            foreach (Cookie c in res.Cookies)
                this.cookies.Add(c);

            uri = new Uri(URL_DATA);
            req = CreateRequest(uri);
            res = (HttpWebResponse)req.GetResponse();
            var rjson = "{}";

            using (var gz = new GZipStream(res.GetResponseStream(), CompressionMode.Decompress))
            using (var sr = new StreamReader(gz, utf))
                rjson = sr.ReadToEnd();

            var json = JObject.Parse(rjson);

            this.data = this.InitData(json);

            this.initialized = true;
        }

        /// <summary>
        /// Initializes the toolkit.
        /// </summary>
        /// <returns>Ongoing operation handle.</returns>
        public async Task InitializeAsync()
        {
            await Task.Run(new Action(this.InitDictionaries));

            var utf = new UTF8Encoding(false);

            var uri = new Uri(URL_BASE);
            var req = WebRequest.CreateHttp(uri);
            req.Headers.Clear();
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            req.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.5");
            req.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");
            req.Headers.Add("DNT: 1");
            req.Host = "skillplanner.stoacademy.com";
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:48.0) Gecko/20100101 Firefox/48.0.1 Waterfox/48.0.1";
            req.CookieContainer = new CookieContainer();

            var res = (HttpWebResponse)(await req.GetResponseAsync());
            foreach (Cookie c in res.Cookies)
                this.cookies.Add(c);

            uri = new Uri(URL_DATA);
            req = CreateRequest(uri);
            res = (HttpWebResponse)(await req.GetResponseAsync());
            var rjson = "{}";

            using (var gz = new GZipStream(res.GetResponseStream(), CompressionMode.Decompress))
            using (var sr = new StreamReader(gz, utf))
                rjson = sr.ReadToEnd();

            var json = JObject.Parse(rjson);

            this.data = await this.InitDataAsync(json);

            this.initialized = true;
        }

        /// <summary>
        /// Fetches build data from STO Academy.
        /// </summary>
        /// <param name="build_id">ID of the build to fetch.</param>
        /// <returns>Processed build.</returns>
        public StoAcademyBuild GetBuild(string build_id)
        {
            if (!this.initialized)
                throw new InvalidOperationException("Initialization is required first");

            var utf = new UTF8Encoding(false);

            var uri = new Uri(string.Concat(URL_BUILD, build_id));
            var req = CreateRequest(uri);
            var res = (HttpWebResponse)req.GetResponse();
            var rjson = "{}";

            using (var gz = new GZipStream(res.GetResponseStream(), CompressionMode.Decompress))
            using (var sr = new StreamReader(gz, utf))
                rjson = sr.ReadToEnd();

            var json = JObject.Parse(rjson);

            var build = this.InitBuild(json);

            return build;
        }

        /// <summary>
        /// Fetches build data from STO Academy.
        /// </summary>
        /// <param name="build_id">ID of the build to fetch.</param>
        /// <returns>Processed build.</returns>
        public async Task<StoAcademyBuild> GetBuildAsync(string build_id)
        {
            if (!this.initialized)
                throw new InvalidOperationException("Initialization is required first");

            var utf = new UTF8Encoding(false);

            var uri = new Uri(string.Concat(URL_BUILD, build_id));
            var req = CreateRequest(uri);
            var res = (HttpWebResponse)(await req.GetResponseAsync());
            var rjson = "{}";

            using (var gz = new GZipStream(res.GetResponseStream(), CompressionMode.Decompress))
            using (var sr = new StreamReader(gz, utf))
                rjson = sr.ReadToEnd();

            var json = JObject.Parse(rjson);

            var build = await this.InitBuildAsync(json);

            return build;
        }

        /// <summary>
        /// Converts ship's Factions flags into a display name.
        /// </summary>
        /// <param name="fac">Faction flag collection.</param>
        /// <returns>Converted display name.</returns>
        public string GetShipFactionsDisplayName(ShipFactions fac)
        {
            var t = typeof(ShipFactions);
            var vs = (ShipFactions[])Enum.GetValues(t);
            var dv = new List<ShipFactions>();
            var sv = new List<string>();

            foreach (var v in vs)
                if (v != ShipFactions.None && ((fac & v) == v))
                    dv.Add(v);

            foreach (var v in dv)
                sv.Add(this.GetEnumDisplayName(v));

            return string.Join(", ", dv);
        }

        /// <summary>
        /// Gets a display name from any enumerated type that supports it.
        /// </summary>
        /// <typeparam name="T">Type of the enumeration.</typeparam>
        /// <param name="enum">Enumeration value.</param>
        /// <returns>Display name for given value.</returns>
        public string GetEnumDisplayName<T>(T @enum) 
            where T : struct, IConvertible
        {
            var t = typeof(T);
            if (!t.IsEnum)
                throw new InvalidOperationException("TEnum must be an enumerated type");

            var dn = @enum.GetAttributeFromEnum<T, DisplayAsAttribute>();
            if (dn == null)
                return @enum.ToString();
            return dn.DisplayName;
        }

        /// <summary>
        /// Gets a display name from a BOFF.
        /// </summary>
        /// <param name="boff">BOFF to format.</param>
        /// <returns>Display name for given boff.</returns>
        public string GetBOFFDisplayName(StoAcademyBoff boff)
        {
            var rnk = this.GetEnumDisplayName(boff.Rank);
            var cab = this.GetEnumDisplayName(boff.Career);
            var cas = this.GetEnumDisplayName(boff.Station.Career == BoffStationCareer.Unknown ? boff.Career : (BoffCareer)boff.Station.Career);
            var spc = this.GetEnumDisplayName(boff.Specialization);

            var cst = "N/A";
            if (boff.Specialization != BoffSpecialization.None)
                cst = string.Concat(cas, "/", spc);
            else
                cst = cas;
            if (cab != cas)
                cst = string.Concat(cst, " (", cab, ")");

            //var cst = boff.Specialization != BOFFSpecialization.None ? string.Concat(cab, "/", spc) : car;
            return string.Concat(rnk, " ", cst);
        }

        // --- PRIVATE ---
        private void InitDictionaries()
        {
            var t = typeof(DoffSpecialization);
            var vss = (DoffSpecialization[])Enum.GetValues(t);
            this.doff_spec_dictionary = new Dictionary<string, DoffSpecialization>();
            foreach (var v in vss)
            {
                var display_name = v.GetAttributeFromEnum<DoffSpecialization, DisplayAsAttribute>();
                if (display_name != null)
                    doff_spec_dictionary[display_name.DisplayName] = v;
            }

            this.item_cache = new Dictionary<int, StoAcademyItem>();

            this.cookies = new List<Cookie>();
        }

        private HttpWebRequest CreateRequest(Uri uri)
        {
            var req = WebRequest.CreateHttp(uri);
            req.Headers.Clear();
            req.Accept = "application/json, text/javascript, */*; q=0.01";
            req.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
            req.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.5");
            req.Headers.Add(HttpRequestHeader.CacheControl, "max-age=0");
            req.Headers.Add("DNT: 1");
            req.Host = "skillplanner.stoacademy.com";
            req.Referer = "http://skillplanner.stoacademy.com/";
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:48.0) Gecko/20100101 Firefox/48.0.1 Waterfox/48.0.1";
            req.Headers.Add("X-Requested-With: XMLHttpRequest");
            req.CookieContainer = new CookieContainer();
            foreach (Cookie c in this.cookies)
            {
                req.CookieContainer.Add(c);
            }

            return req;
        }

        private StoaData InitData(JObject json)
        {
            /* --- Initialize data structures --- */
            // FUSED ITEMS
            var rfi = (JObject)json["fusedItems"];
            var dfi = new Dictionary<int, StoAcademyFusedItem>();

            foreach (var xfi in rfi)
            {
                var fi = (JObject)xfi.Value;

                var id = (int)fi["fused_id"];
                var dn = (string)fi["display_name"];

                dfi.Add(id, new StoAcademyFusedItem
                {
                    ID = id,
                    DisplayName = dn
                });
            }

            var ofi = new ReadOnlyDictionary<int, StoAcademyFusedItem>(dfi);

            // DOFFS
            var rdo = (JArray)json["doffs"];
            var dos = new Dictionary<int, StoAcademyDoff>();
            var reg0 = new Regex(@"(<[a-z]+?>.*?<\/[a-z]+?>|\((Space|Ground)\) )");

            foreach (var xdo in rdo)
            {
                var @do = (JObject)xdo;

                var spec_ = (string)@do["specialization"];
                var reg_ = (string)@do["region"];

                var id = (int)@do["doff_id"];
                var spec = this.doff_spec_dictionary[spec_];
                var reg = reg_ == "none" ? DoffRegion.Agnostic : (reg_ == "space" ? DoffRegion.Space : DoffRegion.Ground);
                var max = (int)@do["max"];
                var ablt = (string)@do["ability"];

                dos.Add(id, new StoAcademyDoff
                {
                    ID = id,
                    Specialization = spec,
                    Region = reg,
                    MaximumCount = max,
                    Ability = ablt
                });
            }

            var oos = new ReadOnlyDictionary<int, StoAcademyDoff>(dos);

            // ABILITIES
            var ras = (JArray)json["abilities"];
            var das = new Dictionary<int, StoAcademyAbility>();

            var _as_td = Enum.GetValues(typeof(AbilityType))
                .Cast<AbilityType>()
                .ToDictionary(xev => xev.ToString().ToLower());
            var _as_rd = Enum.GetValues(typeof(AbilityRank))
                .Cast<AbilityRank>()
                .ToDictionary(xev => xev.ToString().ToLower());
            var _as_cd = Enum.GetValues(typeof(AbilityCareer))
                .Cast<AbilityCareer>()
                .ToDictionary(xev => xev.ToString().ToLower());
            var _as_ld = Enum.GetValues(typeof(AbilityRegion))
                .Cast<AbilityRegion>()
                .ToDictionary(xev => xev.ToString().ToLower());

            foreach (var xas in ras)
            {
                var @as = (JObject)xas;

                var type_ = (string)@as["type"];
                var rank_ = (string)@as["rank"];
                var car_ = (string)@as["career"];
                var reg_ = (string)@as["location"];

                var rank_k = rank_.Replace(" ", "");

                var id = (int)@as["id"];
                var name = (string)@as["name"];
                var type = _as_td.ContainsKey(type_) ? _as_td[type_] : AbilityType.Unknown;
                var rank = _as_rd.ContainsKey(rank_k) ? _as_rd[rank_k] : AbilityRank.Unknown;
                var car = _as_cd.ContainsKey(car_) ? _as_cd[car_] : AbilityCareer.Unknown;
                var reg = _as_ld.ContainsKey(reg_) ? _as_ld[reg_] : AbilityRegion.Unknown;

                das.Add(id, new StoAcademyAbility
                {
                    ID = id,
                    DisplayName = name,
                    Type = type,
                    Rank = rank,
                    Career = car,
                    Region = reg
                });
            }

            var oas = new ReadOnlyDictionary<int, StoAcademyAbility>(das);

            // TRAITS
            var rtt = (JArray)json["traits"];
            var dtt = new Dictionary<int, StoAcademyTrait>();

            var _tt_td = new Dictionary<string, TraitType>()
                {
                    { "active_reputation", TraitType.ReputationActive },
                    { "space_reputation", TraitType.ReputationSpace },
                    { "ground_reputation", TraitType.ReputationGround },
                    { "personal_space", TraitType.PersonalSpace },
                    { "personal_ground", TraitType.PersonalGround },
                    { "starship", TraitType.Starship },
                    { "starship_trait", TraitType.Starship },
                    { "other", TraitType.Other }
                };
            var _tt_fd = Enum.GetValues(typeof(TraitFaction))
                .Cast<TraitFaction>()
                .ToDictionary(xev => xev.ToString().ToLower());
            var _tt_cd = Enum.GetValues(typeof(TraitCareer))
                .Cast<TraitCareer>()
                .ToDictionary(xev => xev.ToString().ToLower());

            foreach (var xtt in rtt)
            {
                var tt = (JObject)xtt;

                var type_ = (string)tt["type"];
                var fac_ = (string)tt["faction"];
                var car_ = (string)tt["career"];

                var fac_k = fac_.ToLower().Replace(" ", "");
                var car_k = car_.ToLower();

                var id = (int)tt["id"];
                var name = (string)tt["name"];
                var type = _tt_td.ContainsKey(type_) ? _tt_td[type_] : TraitType.Unknown;
                var desc_ = (string)tt["description"];
                var desc = reg0.Replace(desc_, "").Trim();
                var fac = _tt_fd.ContainsKey(fac_k) ? _tt_fd[fac_k] : TraitFaction.Unknown;
                var car = _tt_cd.ContainsKey(car_k) ? _tt_cd[car_k] : TraitCareer.Unknown;
                var spec = (string)tt["species"];

                dtt.Add(id, new StoAcademyTrait
                {
                    ID = id,
                    Name = name,
                    Type = type,
                    Description = desc,
                    Faction = fac,
                    Career = car,
                    Species = spec
                });
            }

            var ott = new ReadOnlyDictionary<int, StoAcademyTrait>(dtt);

            // SKILLS
            var rsk = (JArray)json["skills"];
            var dsk = new Dictionary<string, StoAcademySkill>();

            var _sk_ld = Enum.GetValues(typeof(SkillRegion))
                .Cast<SkillRegion>()
                .ToDictionary(xev => xev.ToString().ToLower());
            var _sk_cd = Enum.GetValues(typeof(SkillCareer))
                .Cast<SkillCareer>()
                .ToDictionary(xev => xev.ToString().ToLower());
            var _sk_rd = Enum.GetValues(typeof(SkillRank))
                .Cast<SkillRank>()
                .ToDictionary(xev => xev.ToString().ToLower());
            _sk_rd.Add("lt.commander", SkillRank.LieutenantCommander);

            foreach (var xsk in rsk)
            {
                var sk = (JObject)xsk;

                var reg_ = (string)sk["region"];
                var car_ = (string)sk["career"];
                var rank_ = (string)sk["rank"];
                var desc_ = (string)sk["description"];

                var reg_k = reg_.ToLower();
                var car_k = car_.ToLower();
                var rank_k = rank_.ToLower().Replace(" ", "");

                var id = (int)sk["id"];
                var sel = (string)sk["selector"];
                var selr = (string)sk["requiredSkill"];
                var name = (string)sk["name"];
                var reg = _sk_ld.ContainsKey(reg_k) ? _sk_ld[reg_k] : SkillRegion.Unknown;
                var car = _sk_cd.ContainsKey(car_k) ? _sk_cd[car_k] : SkillCareer.Unknown;
                var rank = _sk_rd.ContainsKey(rank_k) ? _sk_rd[rank_k] : SkillRank.Unknown;
                var desc = reg0.Replace(desc_, "").Trim();
                var bon = (string)sk["bonus"];
                var tbon = (string)sk["totalBonus"];
                var bbon = (string)sk["baseBonus"];

                dsk.Add(sel, new StoAcademySkill
                {
                    ID = id,
                    SkillID = sel,
                    RequiredSkillID = selr,
                    Name = name,
                    Region = reg,
                    Career = car,
                    Rank = rank,
                    Description = desc,
                    Bonus = bon,
                    TotalBonus = tbon,
                    BaseBonus = bbon
                });
            }

            var osk = new ReadOnlyDictionary<string, StoAcademySkill>(dsk);

            // SKILL UNLOCKS
            var dsu = new Dictionary<string, StoAcademySkillUnlock>();

            // eng
            // 5pt
            dsu.Add("EU1-1", new StoAcademySkillUnlock { Name = "Hangar Health", UnlockID = "EU1-1", Career = SkillUnlockCareer.Engineering, PointCount = SkillUnlockPointCount.Points5 });
            dsu.Add("EU1-2", new StoAcademySkillUnlock { Name = "Battery Expertise", UnlockID = "EU1-2", Career = SkillUnlockCareer.Engineering, PointCount = SkillUnlockPointCount.Points5 });
            // 10pt
            dsu.Add("EU2-1", new StoAcademySkillUnlock { Name = "Maximum Hull Capacity", UnlockID = "EU2-1", Career = SkillUnlockCareer.Engineering, PointCount = SkillUnlockPointCount.Points5 });
            dsu.Add("EU2-2", new StoAcademySkillUnlock { Name = "Subsystem Repair", UnlockID = "EU2-2", Career = SkillUnlockCareer.Engineering, PointCount = SkillUnlockPointCount.Points5 });
            // 15pt
            dsu.Add("EU3-1", new StoAcademySkillUnlock { Name = "Engine Subsystem Power", UnlockID = "EU3-1", Career = SkillUnlockCareer.Engineering, PointCount = SkillUnlockPointCount.Points5 });
            dsu.Add("EU3-2", new StoAcademySkillUnlock { Name = "Shield Subsystem Power", UnlockID = "EU3-2", Career = SkillUnlockCareer.Engineering, PointCount = SkillUnlockPointCount.Points5 });
            // 20pt
            dsu.Add("EU4-1", new StoAcademySkillUnlock { Name = "Weapon Subsystem Power", UnlockID = "EU4-1", Career = SkillUnlockCareer.Engineering, PointCount = SkillUnlockPointCount.Points5 });
            dsu.Add("EU4-2", new StoAcademySkillUnlock { Name = "Auxiliary Subsystem Power", UnlockID = "EU4-2", Career = SkillUnlockCareer.Engineering, PointCount = SkillUnlockPointCount.Points5 });
            // ult
            dsu.Add("EU5", new StoAcademySkillUnlock { Name = "EPS Corruption", UnlockID = "EU5", Career = SkillUnlockCareer.Engineering, PointCount = SkillUnlockPointCount.Ultimate });
            // ult enh
            dsu.Add("EU5-1", new StoAcademySkillUnlock { Name = "Weakening Corruption", UnlockID = "EU5-1", Career = SkillUnlockCareer.Engineering, PointCount = SkillUnlockPointCount.UltimateEnhancer });
            dsu.Add("EU5-2", new StoAcademySkillUnlock { Name = "Enhanced Corruption", UnlockID = "EU5-2", Career = SkillUnlockCareer.Engineering, PointCount = SkillUnlockPointCount.UltimateEnhancer });
            dsu.Add("EU5-3", new StoAcademySkillUnlock { Name = "Explosive Corruption", UnlockID = "EU5-3", Career = SkillUnlockCareer.Engineering, PointCount = SkillUnlockPointCount.UltimateEnhancer });
            // sci
            // 5pt
            dsu.Add("SU1-1", new StoAcademySkillUnlock { Name = "Sector Space Travel Speed", UnlockID = "SU1-1", Career = SkillUnlockCareer.Science, PointCount = SkillUnlockPointCount.Points5 });
            dsu.Add("SU1-2", new StoAcademySkillUnlock { Name = "Transwarp Cooldown Reductions", UnlockID = "SU1-2", Career = SkillUnlockCareer.Science, PointCount = SkillUnlockPointCount.Points5 });
            // 10pt
            dsu.Add("SU2-1", new StoAcademySkillUnlock { Name = "Maximum Shield Capacity", UnlockID = "SU2-1", Career = SkillUnlockCareer.Science, PointCount = SkillUnlockPointCount.Points5 });
            dsu.Add("SU2-2", new StoAcademySkillUnlock { Name = "Starship Stealth", UnlockID = "SU2-2", Career = SkillUnlockCareer.Science, PointCount = SkillUnlockPointCount.Points5 });
            // 15pt
            dsu.Add("SU3-1", new StoAcademySkillUnlock { Name = "Starship Perception", UnlockID = "SU3-1", Career = SkillUnlockCareer.Science, PointCount = SkillUnlockPointCount.Points5 });
            dsu.Add("SU3-2", new StoAcademySkillUnlock { Name = "Control Resistance", UnlockID = "SU3-2", Career = SkillUnlockCareer.Science, PointCount = SkillUnlockPointCount.Points5 });
            // 20pt
            dsu.Add("SU4-1", new StoAcademySkillUnlock { Name = "Subsystem Energy Drain Resistance", UnlockID = "SU4-1", Career = SkillUnlockCareer.Science, PointCount = SkillUnlockPointCount.Points5 });
            dsu.Add("SU4-2", new StoAcademySkillUnlock { Name = "Shield Drain Resistance", UnlockID = "SU4-2", Career = SkillUnlockCareer.Science, PointCount = SkillUnlockPointCount.Points5 });
            // ult
            dsu.Add("SU5", new StoAcademySkillUnlock { Name = "Probability Manipulation", UnlockID = "SU5", Career = SkillUnlockCareer.Science, PointCount = SkillUnlockPointCount.Ultimate });
            // ult enh
            dsu.Add("SU5-1", new StoAcademySkillUnlock { Name = "Probability Shell", UnlockID = "SU5-1", Career = SkillUnlockCareer.Science, PointCount = SkillUnlockPointCount.UltimateEnhancer });
            dsu.Add("SU5-2", new StoAcademySkillUnlock { Name = "Probability Penetration", UnlockID = "SU5-2", Career = SkillUnlockCareer.Science, PointCount = SkillUnlockPointCount.UltimateEnhancer });
            dsu.Add("SU5-3", new StoAcademySkillUnlock { Name = "Probability Window", UnlockID = "SU5-3", Career = SkillUnlockCareer.Science, PointCount = SkillUnlockPointCount.UltimateEnhancer });
            // tac
            // 5pt
            dsu.Add("TU1-1", new StoAcademySkillUnlock { Name = "Threat Control", UnlockID = "TU1-1", Career = SkillUnlockCareer.Tactical, PointCount = SkillUnlockPointCount.Points5 });
            dsu.Add("TU1-2", new StoAcademySkillUnlock { Name = "Hangar Weaponry", UnlockID = "TU1-2", Career = SkillUnlockCareer.Tactical, PointCount = SkillUnlockPointCount.Points5 });
            // 10pt
            dsu.Add("TU2-1", new StoAcademySkillUnlock { Name = "Projectile Critical Chance", UnlockID = "TU2-1", Career = SkillUnlockCareer.Tactical, PointCount = SkillUnlockPointCount.Points5 });
            dsu.Add("TU2-2", new StoAcademySkillUnlock { Name = "Projectile Critical Damage", UnlockID = "TU2-2", Career = SkillUnlockCareer.Tactical, PointCount = SkillUnlockPointCount.Points5 });
            // 15pt
            dsu.Add("TU3-1", new StoAcademySkillUnlock { Name = "Energy Critical Chance", UnlockID = "TU3-1", Career = SkillUnlockCareer.Tactical, PointCount = SkillUnlockPointCount.Points5 });
            dsu.Add("TU3-2", new StoAcademySkillUnlock { Name = "Energy Critical Damage", UnlockID = "TU3-2", Career = SkillUnlockCareer.Tactical, PointCount = SkillUnlockPointCount.Points5 });
            // 20pt
            dsu.Add("TU4-1", new StoAcademySkillUnlock { Name = "Accuracy", UnlockID = "TU4-1", Career = SkillUnlockCareer.Tactical, PointCount = SkillUnlockPointCount.Points5 });
            dsu.Add("TU4-2", new StoAcademySkillUnlock { Name = "Defense", UnlockID = "TU4-2", Career = SkillUnlockCareer.Tactical, PointCount = SkillUnlockPointCount.Points5 });
            // ult
            dsu.Add("TU5", new StoAcademySkillUnlock { Name = "Focused Frenzy", UnlockID = "TU5", Career = SkillUnlockCareer.Tactical, PointCount = SkillUnlockPointCount.Ultimate });
            // ult enh
            dsu.Add("TU5-1", new StoAcademySkillUnlock { Name = "Frenzied Reactions", UnlockID = "TU5-1", Career = SkillUnlockCareer.Tactical, PointCount = SkillUnlockPointCount.UltimateEnhancer });
            dsu.Add("TU5-2", new StoAcademySkillUnlock { Name = "Frenzied Assault", UnlockID = "TU5-2", Career = SkillUnlockCareer.Tactical, PointCount = SkillUnlockPointCount.UltimateEnhancer });
            dsu.Add("TU5-3", new StoAcademySkillUnlock { Name = "Team Frenzy", UnlockID = "TU5-3", Career = SkillUnlockCareer.Tactical, PointCount = SkillUnlockPointCount.UltimateEnhancer });
            // ground
            // 1pt
            dsu.Add("EG1-1", new StoAcademySkillUnlock { Name = "Training: Mine Barrier/Hypospray: Dylovene/Photon Grenade III", UnlockID = "EG1-1", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground01 });
            dsu.Add("EG1-2", new StoAcademySkillUnlock { Name = "Training: Turret Fabrication/Medical Tricorder/Smoke Grenade III", UnlockID = "EG1-2", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground01 });
            // 2pt
            dsu.Add("EG2-1", new StoAcademySkillUnlock { Name = "Willpower", UnlockID = "EG2-1", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground02 });
            dsu.Add("EG2-2", new StoAcademySkillUnlock { Name = "Device Expertise", UnlockID = "EG2-2", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground02 });
            // 3pt
            dsu.Add("EG3-1", new StoAcademySkillUnlock { Name = "Training: Quick Fix/Gravimetric Shift/Lunge III", UnlockID = "EG3-1", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground03 });
            dsu.Add("EG3-2", new StoAcademySkillUnlock { Name = "Training: Shield Recharge/Sonic Pulse/Sweeping Strikes III", UnlockID = "EG3-2", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground03 });
            // 4pt
            dsu.Add("EG4-1", new StoAcademySkillUnlock { Name = "Improved Aim", UnlockID = "EG4-1", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground04 });
            dsu.Add("EG4-2", new StoAcademySkillUnlock { Name = "Improved Crouch", UnlockID = "EG4-2", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground04 });
            // 5pt
            dsu.Add("EG5-1", new StoAcademySkillUnlock { Name = "Training: Shield Generator Fabrication/Tricorder Scan/Target Optics III", UnlockID = "EG5-1", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground05 });
            dsu.Add("EG5-2", new StoAcademySkillUnlock { Name = "Training: Medical Generator Fabrication/Dampening Field/Suppressing Fire III", UnlockID = "EG5-2", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground05 });
            // 6pt
            dsu.Add("EG6-1", new StoAcademySkillUnlock { Name = "Furious Footwork", UnlockID = "EG6-1", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground06 });
            dsu.Add("EG6-2", new StoAcademySkillUnlock { Name = "Fatal Fists", UnlockID = "EG6-2", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground06 });
            // 7pt
            dsu.Add("EG7-1", new StoAcademySkillUnlock { Name = "Training: Cover Shield/Neural Neutralizer/Fire On My Mark III", UnlockID = "EG7-1", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground07 });
            dsu.Add("EG7-2", new StoAcademySkillUnlock { Name = "Training: Equipment Diagnostics/Hypospray: Melorazine/Stun Grenade III", UnlockID = "EG7-2", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground07 });
            // 8pt
            dsu.Add("EG8-1", new StoAcademySkillUnlock { Name = "Improved Flank Damage", UnlockID = "EG8-1", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground08 });
            dsu.Add("EG8-2", new StoAcademySkillUnlock { Name = "Improved Flank Resistance", UnlockID = "EG8-2", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground08 });
            // 9pt
            dsu.Add("EG9-1", new StoAcademySkillUnlock { Name = "Training: Combat Supply/Nanite Health Monitor/Overwatch III", UnlockID = "EG9-1", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground09 });
            dsu.Add("EG9-2", new StoAcademySkillUnlock { Name = "Training: Support Drone Fabrication/Vascular Regenerator/Stealth Module III", UnlockID = "EG9-2", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground09 });
            // 10pt
            dsu.Add("EG10-1", new StoAcademySkillUnlock { Name = "Offensive Mastery", UnlockID = "EG10-1", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground10 });
            dsu.Add("EG10-2", new StoAcademySkillUnlock { Name = "Defensive Mastery", UnlockID = "EG10-2", Career = SkillUnlockCareer.Unknown, PointCount = SkillUnlockPointCount.Ground10 });

            var osu = new ReadOnlyDictionary<string, StoAcademySkillUnlock>(dsu);

            // TIERS
            var rtr = (JArray)json["tiers"];
            var dtr = new Dictionary<int, StoAcademyTier>();

            foreach (var xtr in rtr)
            {
                var tr = (JObject)xtr;

                var id = (int)tr["id"];
                var ord = (int)tr["tier_order"];
                var lvl = (int)tr["level"];
                var name = (string)tr["federation"];

                dtr.Add(id, new StoAcademyTier
                {
                    ID = id,
                    Order = ord,
                    Level = lvl,
                    Name = name
                });
            }

            var otr = new ReadOnlyDictionary<int, StoAcademyTier>(dtr);

            // SHIPS
            var rss = (JArray)json["ships"];
            var dss = new Dictionary<int, STOAcademyStarship>();

            var _ss_kd = Enum.GetValues(typeof(ShipType))
                .Cast<ShipType>()
                .ToDictionary(xev => xev.ToString().ToLower());

            var _ss_st_cd = Enum.GetValues(typeof(BoffStationCareer))
                .Cast<BoffStationCareer>()
                .ToDictionary(xev => xev.ToString().ToLower());
            var _ss_st_rd = Enum.GetValues(typeof(BoffStationRank))
                .Cast<BoffStationRank>()
                .ToDictionary(xev => xev.ToString().ToLower());
            var _ss_st_sd = Enum.GetValues(typeof(BoffStationSpecialization))
                .Cast<BoffStationSpecialization>()
                .ToDictionary(xev => xev.ToString().ToLower());

            foreach (var xss in rss)
            {
                try
                {
                    var ss = (JObject)xss;

                    var type_ = (string)ss["type"];
                    var tier_ = (int)ss["tier_id"];
                    var fac0_ = (int)ss["federation"];
                    var fac1_ = (int)ss["klingon_empire"];
                    var fac2_ = (int)ss["romulan_republic"];
                    var fac_ = (fac0_ == 1 ? ShipFactions.Federation : ShipFactions.None) |
                        (fac1_ == 1 ? ShipFactions.KlingonEmpire : ShipFactions.None) |
                        (fac2_ == 1 ? ShipFactions.RomulanRepublic : ShipFactions.None);
                    var secdef_ = (int)ss["deflectors"];
                    var fused0_ = (string)ss["fused_items"];
                    var fused1_ = !string.IsNullOrWhiteSpace(fused0_) ? int.Parse(fused0_.Split('=')[1]) : -1;
                    var fused_ = ofi.ContainsKey(fused1_) ? ofi[fused1_] : default(StoAcademyFusedItem);
                    var bsts_ = new List<StoAcademyBoffStation>();
                    for (int i = 1; i <= 6; i++)
                    {
                        var bsr_ = (string)ss[string.Concat("boff", i, "_rank")];
                        var bsc_ = (string)ss[string.Concat("boff", i, "_career")];
                        var bss_ = (string)ss[string.Concat("boff", i, "_spec")];

                        if (string.IsNullOrWhiteSpace(bsr_))
                            break;

                        var bsr_k = bsr_.ToLower().Replace(" ", "");
                        var bsc_k = bsc_.ToLower();
                        var bss_k = bss_.ToLower().Replace(" ", "");

                        var bsr = _ss_st_rd.ContainsKey(bsr_k) ? _ss_st_rd[bsr_k] : BoffStationRank.Unknown;
                        var bsc = _ss_st_cd.ContainsKey(bsc_k) ? _ss_st_cd[bsc_k] : BoffStationCareer.Unknown;
                        var bss = _ss_st_sd.ContainsKey(bss_k) ? _ss_st_sd[bss_k] : (string.IsNullOrWhiteSpace(bss_k) ? BoffStationSpecialization.None : BoffStationSpecialization.Unknown);

                        bsts_.Add(new StoAcademyBoffStation
                        {
                            Rank = bsr,
                            Career = bsc,
                            Specialization = bss
                        });
                    }

                    var type_k = type_.ToLower().Replace(" ", "").Replace("-", "");

                    var id = (int)ss["id"];
                    var name = (string)ss["name"];
                    var type = _ss_kd.ContainsKey(type_k) ? _ss_kd[type_k] : ShipType.Unknown;
                    var tier = otr[tier_];
                    var fac = fac_;
                    var secdef = secdef_ == 2;
                    var fguns = (int)ss["fore_weapons"];
                    var aguns = (int)ss["aft_weapons"];
                    var hull = (int)ss["base_hull"];
                    var smod = (float)ss["shield_modifier"];
                    var turn = (float)ss["base_turn"];
                    var dslt = (int)ss["device_slots"];
                    var ceng = (int)ss["engineering_consoles"];
                    var csci = (int)ss["science_consoles"];
                    var ctac = (int)ss["tactical_consoles"];
                    var hang = (int)ss["hangars"];
                    var fused = fused_;
                    var wpwr = (int)ss["weapon_power"];
                    var spwr = (int)ss["shield_power"];
                    var epwr = (int)ss["engine_power"];
                    var apwr = (int)ss["auxiliary_power"];
                    var bsts = new ReadOnlyCollection<StoAcademyBoffStation>(bsts_);

                    dss.Add(id, new STOAcademyStarship
                    {
                        ID = id,
                        Name = name,
                        Type = type,
                        Tier = tier,
                        Factions = fac,
                        HasSecondaryDeflector = secdef,
                        ForeWeapons = fguns,
                        AftWeapons = aguns,
                        BaseHull = hull,
                        ShieldModifier = smod,
                        BaseTurnRate = turn,
                        DeviceSlots = dslt,
                        ConsoleSlotsEngineering = ceng,
                        ConsoleSlotsScience = csci,
                        ConsoleSlotsTactical = ctac,
                        Hangars = hang,
                        FusedItem = fused,
                        BonusWeaponPower = wpwr,
                        BonusShieldPower = spwr,
                        BonusEnginePower = epwr,
                        BonuxAuxiliaryPower = apwr,
                        BOFFStations = bsts
                    });
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            var oss = new ReadOnlyDictionary<int, STOAcademyStarship>(dss);
            /* --- ENDOF --- */

            var data = new StoaData
            {
                AllAbilities = oas,
                AllDOFFs = oos,
                AllFusedItems = ofi,
                AllShips = oss,
                AllSkills = osk,
                AllSkillUnlocks = osu,
                AllTiers = otr,
                AllTraits = ott
            };

            return data;
        }

        private async Task<StoaData> InitDataAsync(JObject json)
        {
            return await Task.Run(new Func<StoaData>(() =>
            {
                return this.InitData(json);
            }));
        }

        private StoAcademyBuild InitBuild(JObject json)
        {
            var owner = (string)json["owner"];
            var name = (string)json["build_name"];
            var id = (string)json["build_url"];

            var _b_cd = Enum.GetValues(typeof(BuildCareer))
                .Cast<BuildCareer>()
                .ToDictionary(xev => xev.ToString().ToLower());
            var _b_fd = Enum.GetValues(typeof(BuildFaction))
                .Cast<BuildFaction>()
                .ToDictionary(xev => xev.ToString().ToLower());
            var _b_pd = Enum.GetValues(typeof(BuildSpecializationPrimary))
                .Cast<BuildSpecializationPrimary>()
                .ToDictionary(xev => xev.ToString().ToLower());
            var _b_sd = Enum.GetValues(typeof(BuildSpecializationSecondary))
                .Cast<BuildSpecializationSecondary>()
                .ToDictionary(xev => xev.ToString().ToLower());

            var car_ = (string)json["career"];
            car_ = car_.ToLower().Replace(" ", "");
            var car = _b_cd.ContainsKey(car_) ? _b_cd[car_] : BuildCareer.Unknown;

            var fac_ = (string)json["faction"];
            fac_ = fac_.ToLower().Replace(" ", "");
            var fac = _b_fd.ContainsKey(fac_) ? _b_fd[fac_] : BuildFaction.Unknown;

            var spcs = (string)json["species"];

            var items_rjson = (string)json["items"];
            var boffs_rjson = (string)json["boffs"];
            var doffs_rjson = (string)json["doffs"];
            var trts_rjson = (string)json["traits"];
            var skls_rjson = (string)json["skills"];
            var skus_rjson = (string)json["skillUnlocks"];

            var ship_ = (int)json["shipId"];
            var ship = this.data.AllShips.ContainsKey(ship_) ? this.data.AllShips[ship_] : default(STOAcademyStarship);

            var pspec_ = (string)json["primarySpec"];
            pspec_ = pspec_.ToLower().Replace(" ", "");
            var pspec = _b_pd.ContainsKey(pspec_) ? _b_pd[pspec_] : BuildSpecializationPrimary.Unknown;

            var sspec_ = (string)json["secondarySpec"];
            sspec_ = sspec_.ToLower().Replace(" ", "");
            var sspec = _b_sd.ContainsKey(sspec_) ? _b_sd[sspec_] : BuildSpecializationSecondary.Unknown;

            var desc = (string)json["description"];
            var notes = (string)json["notes"];
            
            var items_json = !string.IsNullOrWhiteSpace(items_rjson) ? JObject.Parse(items_rjson) : new JObject();
            var boffs_json = !string.IsNullOrWhiteSpace(boffs_rjson) ? JObject.Parse(boffs_rjson) : new JObject();
            var doffs_json = !string.IsNullOrWhiteSpace(doffs_rjson) ? JObject.Parse(doffs_rjson) : new JObject();
            var trts_json = !string.IsNullOrWhiteSpace(trts_rjson) ? JObject.Parse(trts_rjson) : new JObject();
            var skls_json = !string.IsNullOrWhiteSpace(skls_rjson) ? JArray.Parse(skls_rjson) : new JArray();
            var skus_json = !string.IsNullOrWhiteSpace(skus_rjson) ? JArray.Parse(skus_rjson) : new JArray();

            var itemmaps = new Dictionary<string, int>()
                {
                    { "foreWeapon", ship.ForeWeapons },
                    { "aftWeapon", ship.AftWeapons },
                    { "deflectorArray", 1 },
                    { "secondaryDeflector", ship.HasSecondaryDeflector ? 1 : 0 },
                    { "impulseEngines", 1 },
                    { "core", 1 },
                    { "shieldArray", 1 },
                    { "spaceDevice", ship.DeviceSlots },
                    { "spaceEngineeringConsole", ship.ConsoleSlotsEngineering },
                    { "spaceScienceConsole", ship.ConsoleSlotsScience },
                    { "spaceTacticalConsole", ship.ConsoleSlotsTactical },
                    { "spaceHangar", ship.Hangars }
                };
            var itemtypemaps = new Dictionary<string, BuildItemType>()
                {
                    { "foreWeapon", BuildItemType.ForeWeapon },
                    { "aftWeapon", BuildItemType.AftWeapon },
                    { "deflectorArray", BuildItemType.Deflector },
                    { "secondaryDeflector", BuildItemType.SecondaryDeflector },
                    { "impulseEngines", BuildItemType.ImpulseEngine },
                    { "core", BuildItemType.CoreWarp },
                    { "shieldArray", BuildItemType.Shields },
                    { "spaceDevice", BuildItemType.Device },
                    { "spaceEngineeringConsole", BuildItemType.ConsoleEngineering },
                    { "spaceScienceConsole", BuildItemType.ConsoleScience },
                    { "spaceTacticalConsole", BuildItemType.ConsoleTactical },
                    { "spaceHangar", BuildItemType.Hangar }
                };
            var itemmapsG = new Dictionary<string, int>()
                {
                    { "groundKit", 1 },
                    { "groundKitmodule", 5 },
                    { "groundArmor", 1 },
                    { "groundShield", 1 },
                    { "groundWeapon", 2 },
                    { "groundDevice", 1 },
                };
            var itemtypemapsG = new Dictionary<string, BuildItemType>()
                {
                    { "groundKit", BuildItemType.Kit },
                    { "groundKitmodule", BuildItemType.KitModule },
                    { "groundArmor", BuildItemType.Armor },
                    { "groundShield", BuildItemType.PersonalShield },
                    { "groundWeapon", BuildItemType.Weapon },
                    { "groundDevice", BuildItemType.GroundDevice },
                };
            var its_ = new List<StoAcademyBuildItem>();
            var itg_ = new List<StoAcademyBuildItem>();
            foreach (var itemmap in itemmaps)
            {
                for (int i = 1; i <= itemmap.Value; i++)
                {
                    var item__ = (string)items_json[string.Concat(itemmap.Key, i)];
                    if (item__ == null)
                        continue;

                    var item_ = (int)items_json[string.Concat(itemmap.Key, i)];
                    var item = this.GetItem(item_);

                    var type = itemtypemaps.ContainsKey(itemmap.Key) ? itemtypemaps[itemmap.Key] : BuildItemType.Unknown;
                    //if (itemmap.Key == "deflectorArray")
                    //    type = itemmap.Value == 1 ? BuildItemType.Deflector : BuildItemType.SecondaryDeflector;

                    var bitem = new StoAcademyBuildItem
                    {
                        Item = item,
                        ItemType = type
                    };

                    its_.Add(bitem);
                }
            }
            foreach (var itemmap in itemmapsG)
            {
                for (int i = 1; i <= itemmap.Value; i++)
                {
                    var item__ = (string)items_json[string.Concat(itemmap.Key, i)];
                    if (item__ == null)
                        continue;

                    var item_ = (int)items_json[string.Concat(itemmap.Key, i)];
                    var item = this.GetItem(item_);

                    var type = itemtypemapsG.ContainsKey(itemmap.Key) ? itemtypemapsG[itemmap.Key] : BuildItemType.Unknown;

                    var bitem = new StoAcademyBuildItem
                    {
                        Item = item,
                        ItemType = type
                    };

                    itg_.Add(bitem);
                }
            }
            var its = new ReadOnlyCollection<StoAcademyBuildItem>(its_);
            var itg = new ReadOnlyCollection<StoAcademyBuildItem>(itg_);

            var boffs = new ReadOnlyCollection<StoAcademyBoff>(new StoAcademyBoff[0]);
            if (!string.IsNullOrWhiteSpace(ship.Name))
            {
                var xboffs_json = (JObject)boffs_json[ship.Name];
                var boffs_ = new List<StoAcademyBoff>();
                var ranks = new Dictionary<string, BoffRank>()
                {
                    { "ensign", BoffRank.Ensign },
                    { "lieutenant", BoffRank.Lieutenant },
                    { "lieutenant commander", BoffRank.LieutenantCommander },
                    { "commander", BoffRank.Commander },
                };
                var _b_b_cd = Enum.GetValues(typeof(BoffCareer))
                    .Cast<BoffCareer>()
                    .ToDictionary(xev => xev.ToString().ToLower());
                var _b_b_sd = Enum.GetValues(typeof(BoffSpecialization))
                    .Cast<BoffSpecialization>()
                    .ToDictionary(xev => xev.ToString().ToLower());
                for (int i = 1; i <= 6; i++)
                {
                    var xboff = xboffs_json[string.Concat("boff", i)];

                    if (xboff != null)
                    {
                        var boff_ = (JObject)xboff;
                        var station = ship.BOFFStations.ElementAt(i - 1);

                        var _b_car_ = (string)boff_["career"];
                        _b_car_ = _b_car_.ToLower();
                        var _b_car = _b_b_cd.ContainsKey(_b_car_) ? _b_b_cd[_b_car_] : BoffCareer.Unknown;

                        var abs_ = new List<StoAcademyAbility>();
                        foreach (var rank in ranks)
                        {
                            var rab__ = (string)boff_[rank.Key];
                            if (rab__ == null) break;
                            var rab_ = (int)boff_[rank.Key];

                            var rab = data.AllAbilities[rab_];
                            abs_.Add(rab);
                        }
                        var abs = new ReadOnlyCollection<StoAcademyAbility>(abs_);

                        var st = ship.BOFFStations.ElementAt(i - 1);

                        var boff = new StoAcademyBoff
                        {
                            Career = _b_car,
                            Rank = (BoffRank)station.Rank,
                            Specialization = (BoffSpecialization)station.Specialization,
                            Abilities = abs,
                            Station = st
                        };
                        boffs_.Add(boff);
                    }
                }
                boffs = new ReadOnlyCollection<StoAcademyBoff>(boffs_);
            }

            var boffs_away = boffs_json["awayteam"] as JObject;
            var awayteam = new ReadOnlyCollection<StoAcademyBoff>(new StoAcademyBoff[0]);
            if (boffs_away != null)
            {
                var awayteam_ = new List<StoAcademyBoff>();
                var ranks = new Dictionary<string, BoffRank>()
                {
                    { "ensign", BoffRank.Ensign },
                    { "lieutenant", BoffRank.Lieutenant },
                    { "lieutenant commander", BoffRank.LieutenantCommander },
                    { "commander", BoffRank.Commander },
                };
                var _b_b_cd = Enum.GetValues(typeof(BoffCareer))
                    .Cast<BoffCareer>()
                    .ToDictionary(xev => xev.ToString().ToLower());
                var _b_b_sd = Enum.GetValues(typeof(BoffSpecialization))
                    .Cast<BoffSpecialization>()
                    .ToDictionary(xev => xev.ToString().ToLower());
                for (int i = 1; i <= 4; i++)
                {
                    var xboffG = boffs_away[string.Concat("awayteam_", i)];
                    if (xboffG != null)
                    {
                        var boff_ = (JObject)xboffG;

                        var _b_car_ = (string)boff_["career"];
                        _b_car_ = _b_car_.ToLower();
                        var _b_car = _b_b_cd.ContainsKey(_b_car_) ? _b_b_cd[_b_car_] : BoffCareer.Unknown;

                        var _b_spc_ = (string)boff_["spec"];
                        _b_spc_ = _b_spc_ != null ? _b_spc_.ToLower() : "";
                        var _b_spc = _b_b_sd.ContainsKey(_b_spc_) ? _b_b_sd[_b_spc_] : BoffSpecialization.None;

                        var abs_ = new List<StoAcademyAbility>();
                        foreach (var rank in ranks)
                        {
                            var rab__ = (string)boff_[rank.Key];
                            if (rab__ == null) break;
                            var rab_ = (int)boff_[rank.Key];

                            var rab = data.AllAbilities[rab_];
                            abs_.Add(rab);
                        }
                        var abs = new ReadOnlyCollection<StoAcademyAbility>(abs_);

                        var boff = new StoAcademyBoff
                        {
                            Career = _b_car,
                            Rank = BoffRank.Commander,
                            Specialization = _b_spc,
                            Abilities = abs,
                            Station = default(StoAcademyBoffStation)
                        };
                        awayteam_.Add(boff);
                    }
                }
                awayteam = new ReadOnlyCollection<StoAcademyBoff>(awayteam_);
            }

            var doffs_ = new List<StoAcademyBuildDoff>();
            var _b_d_qd = Enum.GetValues(typeof(BuildDoffRarity))
                .Cast<BuildDoffRarity>()
                .ToDictionary(xev => xev.ToString().ToLower());
            for (int i = 1; i <= 6; i++)
            {
                var xdoff = doffs_json[string.Concat("doffS", i)];
                var xdoffG = doffs_json[string.Concat("doffG", i)];

                if (xdoff != null)
                {
                    var doff_ = (JObject)xdoff;

                    var do_ = (int)doff_["id"];
                    var @do = this.data.AllDOFFs[do_];

                    var qlty_ = (string)doff_["rarity"];
                    qlty_ = qlty_.ToLower().Replace(" ", "").Replace("unique", "epic");
                    var qlty = _b_d_qd.ContainsKey(qlty_) ? _b_d_qd[qlty_] : BuildDoffRarity.Unknown;

                    var doff = new StoAcademyBuildDoff
                    {
                        DOFF = @do,
                        Rarity = qlty
                    };

                    doffs_.Add(doff);
                }
                
                if (xdoffG != null)
                {
                    var doff_ = (JObject)xdoffG;

                    var do_ = (int)doff_["id"];
                    var @do = this.data.AllDOFFs[do_];

                    var qlty_ = (string)doff_["rarity"];
                    qlty_ = qlty_.ToLower().Replace(" ", "").Replace("unique", "epic");
                    var qlty = _b_d_qd.ContainsKey(qlty_) ? _b_d_qd[qlty_] : BuildDoffRarity.Unknown;

                    var doff = new StoAcademyBuildDoff
                    {
                        DOFF = @do,
                        Rarity = qlty
                    };

                    doffs_.Add(doff);
                }
            }
            var doffs = new ReadOnlyCollection<StoAcademyBuildDoff>(doffs_);

            var _traitmap = new Dictionary<string, int>()
                {
                    { "personalSpace", 10 },
                    { "personalGround", 10 },
                    { "starship", 5 },
                    { "spaceReutation", 5 },
                    { "groundReutation", 5 },
                    { "activeReutation", 5 },
                    { "other", 2 },
                };
            var trts_ = new List<StoAcademyTrait>();
            foreach (var traitmap in _traitmap)
            {
                for (int i = 1; i <= traitmap.Value; i++)
                {
                    var trt__ = (string)trts_json[string.Concat(traitmap.Key, "Trait", i)];
                    if (trt__ == null)
                        continue;
                    var trt_ = (int)trts_json[string.Concat(traitmap.Key, "Trait", i)];
                    var trt = this.data.AllTraits[trt_];
                    trts_.Add(trt);
                }
            }
            var trts = new ReadOnlyCollection<StoAcademyTrait>(trts_);

            var skls_ = new List<StoAcademySkill>();
            foreach (var skill_ in skls_json)
            {
                var skl_ = (string)skill_;
                var skl = this.data.AllSkills[skl_];
                skls_.Add(skl);
            }
            var skls = new ReadOnlyCollection<StoAcademySkill>(skls_);

            var skus_ = new List<StoAcademySkillUnlock>();
            foreach (var sku__ in skus_json)
            {
                var sku_ = (string)sku__;
                var sku = this.data.AllSkillUnlocks[sku_];
                skus_.Add(sku);
            }
            var skus = new ReadOnlyCollection<StoAcademySkillUnlock>(skus_);

            var build = new StoAcademyBuild
            {
                Owner = owner,
                Name = name,
                ID = id,
                Career = car,
                Faction = fac,
                Species = spcs,
                BuildItems = its,
                BuildEquipment = itg,
                BOFFs = boffs,
                AwayTeam = awayteam,
                DOFFs = doffs,
                Ship = ship,
                Traits = trts,
                Skills = skls,
                SkillUnlocks = skus,
                SpecializationPrimary = pspec,
                SpecializationSecondary = sspec,
                Description = desc,
                Notes = notes
            };

            return build;
        }

        private async Task<StoAcademyBuild> InitBuildAsync(JObject json)
        {
            return await Task.Run(() =>
            {
                return this.InitBuild(json);
            });
        }

        private StoAcademyItem GetItem(int id)
        {
            if (this.item_cache.ContainsKey(id))
                return this.item_cache[id];
            
            var utf = new UTF8Encoding(false);
            var uri = new Uri(string.Concat(URL_ITEM, id));
            var req = CreateRequest(uri);
            var res = (HttpWebResponse)req.GetResponse();
            var rjson = "{}";

            using (var gz = new GZipStream(res.GetResponseStream(), CompressionMode.Decompress))
            using (var sr = new StreamReader(gz, utf))
                rjson = sr.ReadToEnd();

            var json = JArray.Parse(rjson);
            var item_ = (JObject)json[0];

            var _qd = Enum.GetValues(typeof(ItemRarity))
                .Cast<ItemRarity>()
                .ToDictionary(xev => xev.ToString().ToLower());

            var mods__ = (string)item_["modifiers"];
            var mods_ = mods__.Split(' ').ToArray();

            var qlty_ = (string)item_["rarity"];
            qlty_ = qlty_.ToLower().Replace(" ", "");

            var iid = (int)item_["id"];
            var pn = (string)item_["partName"];
            var dn = (string)item_["display_name"];
            var mods = new ReadOnlyCollection<string>(mods_.Where(xs => !string.IsNullOrWhiteSpace(xs)).ToArray());
            var mk = (string)item_["mk"];
            var qlty = _qd.ContainsKey(qlty_) ? _qd[qlty_] : ItemRarity.Unknown;

            var fn = pn;
            if (!string.IsNullOrWhiteSpace(mk))
                fn = string.Concat(fn, " ", mk);
            if (mods.Count > 0)
            {
                var mds = mods
                    .OrderBy(xs => xs);
                //  .GroupBy(xs => xs);

                //var ms = string.Join(" ", mds.Select(xsg => xsg.Count() > 1 ? string.Concat(xsg.Key, "x", xsg.Count()) : xsg.Key));
                var ms = string.Join(" ", mds);
                fn = string.Concat(fn, " ", ms);
            }

            var item = new StoAcademyItem
            {
                ID = iid,
                Name = pn,
                DisplayName = dn,
                Modifiers = mods,
                Mark = mk,
                Rarity = qlty,
                FullName = fn
            };
            this.item_cache[iid] = item;

            return this.item_cache[id];
        }
    }
}
