using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Emzi0767.Gaming.Sto.StoaLib;
using Emzi0767.Gaming.Sto.StoaLib.Data;
using Emzi0767.Gaming.Sto.StoaLib.Data.Enums;

namespace Emzi0767.Gaming.Sto.Abt2.Utility
{
    public class BuildWriter : IDisposable
    {
        private MarkdownWriter MDW { get; set; }

        public BuildWriter(MarkdownWriter mdw)
        {
            this.MDW = mdw;
        }

        public void WriteBuild(StoAcademyBuild build, StoAcademyTools tools, BuildType type)
        {
            if (type == BuildType.Space)
                this.WriteBuildSpace(build, tools);
            else
                this.WriteBuildGround(build, tools);
        }

        private void WriteBuildSpace(StoAcademyBuild build, StoAcademyTools tools)
        {
            this.MDW.WriteHeaderLink(build.Name, build.Url, 1);
            this.MDW.WriteRule();

            var ciths = new string[] { "Category", "Data" };
            var cialn = new string[] { "left", "left" };
            var cidat = new string[][]
            {
                new string[] { "*Captain Career*", tools.GetEnumDisplayName(build.Career) },
                new string[] { "*Captain Faction*", tools.GetEnumDisplayName(build.Faction) },
                new string[] { "*Primary Specialization*", tools.GetEnumDisplayName(build.SpecializationPrimary) },
                new string[] { "*Secondary Specialization*", tools.GetEnumDisplayName(build.SpecializationSecondary) },
            };
            this.MDW.WriteHeader("Captain Info", 2);
            this.MDW.WriteTable(ciths, cialn, cidat);
            this.MDW.WriteRule();

            var skills = this.ReduceSkills(build.Skills).GroupBy(xsk => xsk.Rank);
            if (skills.Count() > 0)
            {
                this.MDW.WriteHeader("Skill Tree", 2);
                this.MDW.StartTable();
                this.MDW.WriteTableHeaders(new string[] { "Rank", "Engineering", "Science", "Tactical" });
                this.MDW.WriteTableAlignments(new string[] { "left", "center", "center", "center" });
                foreach (var skill_lvl in skills)
                {
                    var skillc = skill_lvl.GroupBy(xsk => xsk.Career);
                    
                    var se = skillc.FirstOrDefault(xsk => xsk.Key == SkillCareer.Engineering);
                    var ss = skillc.FirstOrDefault(xsk => xsk.Key == SkillCareer.Science);
                    var st = skillc.FirstOrDefault(xsk => xsk.Key == SkillCareer.Tactical);

                    var ce = se != null ? se.Count() : 0;
                    var cs = ss != null ? ss.Count() : 0;
                    var ct = st != null ? st.Count() : 0;

                    var max = ce > cs ? ce : cs;
                    max = max > ct ? max : ct;

                    var rw = false;

                    for (int i = 0; i < max; i++)
                    {
                        var xse = se != null ? se.ElementAtOrDefault(i) : default(StoAcademySkill);
                        var xss = ss != null ? ss.ElementAtOrDefault(i) : default(StoAcademySkill);
                        var xst = st != null ? st.ElementAtOrDefault(i) : default(StoAcademySkill);

                        this.MDW.StartTableRow();

                        if (!rw)
                        {
                            if (skill_lvl.Key != SkillRank.Lieutenant)
                            {
                                this.MDW.WriteTableCell("");
                                this.MDW.WriteTableCell("");
                                this.MDW.WriteTableCell("");
                                this.MDW.WriteTableCell("");
                                this.MDW.EndTableRow();
                                this.MDW.StartTableRow();
                            }

                            this.MDW.WriteFormat(tools.GetEnumDisplayName(skill_lvl.Key), true, false, false);
                            rw = true;
                        }
                        this.MDW.WriteTableCell("");

                        if (xse.SkillID != null)
                            this.MDW.WriteTableCell(xse.Name);
                        else
                            this.MDW.WriteTableCell("");
                        if (xss.SkillID != null)
                            this.MDW.WriteTableCell(xss.Name);
                        else
                            this.MDW.WriteTableCell("");
                        if (xst.SkillID != null)
                            this.MDW.WriteTableCell(xst.Name);
                        else
                            this.MDW.WriteTableCell("");

                        this.MDW.EndTableRow();
                    }
                }
                this.MDW.StartTableRow();
                this.MDW.WriteTableCell("");
                this.MDW.WriteTableCell("");
                this.MDW.WriteTableCell("");
                this.MDW.WriteTableCell("");
                this.MDW.EndTableRow();
                this.MDW.StartTableRow();
                this.MDW.WriteTableCell("**Total**");
                this.MDW.WriteTableCell(build.Skills.Count(xsk => xsk.Career == SkillCareer.Engineering).ToString());
                this.MDW.WriteTableCell(build.Skills.Count(xsk => xsk.Career == SkillCareer.Science).ToString());
                this.MDW.WriteTableCell(build.Skills.Count(xsk => xsk.Career == SkillCareer.Tactical).ToString());
                this.MDW.EndTableRow();
                this.MDW.EndTable();

                var unlocks = build.SkillUnlocks.OrderBy(xsku => xsku.UnlockID);
                if (unlocks.Count() > 0)
                {
                    string[] purchases = { "**5**", "**10**", "**15**", "**20**", "**24 (Ultimate)**", "**25 (1st Ultimate Enhancer)**", "**26 (2nd Ultimate Enhancer)**", "**27 (3rd Ultimate Enhancer)**" };
                    var ue = unlocks != null ? unlocks.Where(xsku => xsku.Career == SkillUnlockCareer.Engineering) : null;
                    var us = unlocks != null ? unlocks.Where(xsku => xsku.Career == SkillUnlockCareer.Science) : null;
                    var ut = unlocks != null ? unlocks.Where(xsku => xsku.Career == SkillUnlockCareer.Tactical) : null;
                    this.MDW.WriteHeader("Space Unlocks", 3);
                    this.MDW.StartTable();
                    this.MDW.WriteTableHeaders(new string[] { "Purchases", "Engineering", "Science", "Tactical" });
                    this.MDW.WriteTableAlignments(new string[] { "left", "center", "center", "center" });
                    for (int i = 0; i < purchases.Length; i++)
                    {
                        var xue = ue != null ? ue.ElementAtOrDefault(i) : default(StoAcademySkillUnlock);
                        var xus = us != null ? us.ElementAtOrDefault(i) : default(StoAcademySkillUnlock);
                        var xut = ut != null ? ut.ElementAtOrDefault(i) : default(StoAcademySkillUnlock);

                        if (xue.UnlockID == null && xus.UnlockID == null && xut.UnlockID == null)
                            break;

                        this.MDW.StartTableRow();

                        this.MDW.WriteTableCell(purchases[i]);
                        if (xue.UnlockID != null)
                            this.MDW.WriteTableCell(xue.Name);
                        else
                            this.MDW.WriteTableCell("");
                        if (xus.UnlockID != null)
                            this.MDW.WriteTableCell(xus.Name);
                        else
                            this.MDW.WriteTableCell("");
                        if (xut.UnlockID != null)
                            this.MDW.WriteTableCell(xut.Name);
                        else
                            this.MDW.WriteTableCell("");

                        this.MDW.EndTableRow();
                    }
                    this.MDW.EndTable();
                }
                this.MDW.WriteRule();
            }

            if (!string.IsNullOrWhiteSpace(build.Description))
            {
                this.MDW.WriteHeader("Build Description", 2);
                this.MDW.WriteHtml(build.Description);
                this.MDW.WriteRule();
            }

            if (!string.IsNullOrWhiteSpace(build.Ship.Name))
            {
                var siths = new string[] { "Category", "Data" };
                var sialn = new string[] { "left", "left" };
                var sidat = new string[][]
                {
                new string[] { "*Ship Model*", build.Ship.Name },
                };
                this.MDW.WriteHeader("Starship Info", 2);
                this.MDW.WriteTable(siths, sialn, sidat);
                this.MDW.WriteRule();

                var components = build.BuildItems.OrderBy(xbi => xbi.ItemType);
                if (components.Count() > 0)
                {
                    this.MDW.WriteHeader("Starship Loadout", 2);
                    this.MDW.StartTable();
                    this.MDW.WriteTableHeaders(new string[] { "Slot", "Component", "Rarity" });
                    this.MDW.WriteTableAlignments(new string[] { "left", "left", "left" });
                    var ptype = BuildItemType.Unknown;
                    foreach (var item in components)
                    {
                        this.MDW.StartTableRow();
                        if (item.ItemType != ptype)
                        {
                            if (ptype != BuildItemType.Unknown)
                            {
                                this.MDW.WriteTableCell("");
                                this.MDW.WriteTableCell("");
                                this.MDW.WriteTableCell("");
                                this.MDW.EndTableRow();
                                this.MDW.StartTableRow();
                            }

                            ptype = item.ItemType;
                            this.MDW.WriteFormat(tools.GetEnumDisplayName(item.ItemType), true, false, false);
                        }
                        this.MDW.WriteTableCell("");
                        this.MDW.WriteTableCell(item.Item.FullName);
                        this.MDW.WriteTableCell(tools.GetEnumDisplayName(item.Item.Rarity));
                        this.MDW.EndTableRow();
                    }
                    this.MDW.EndTable();
                    this.MDW.WriteRule();
                }
            }

            if ((build.BOFFs.Count() > 0 && build.BOFFs.SelectMany(xbo => xbo.Abilities).Count() > 0) || build.DOFFs.Where(xdo => xdo.DOFF.Region == DoffRegion.Space).Count() > 0)
            {
                this.MDW.WriteHeader("Officers and Crew", 2);
                if (build.BOFFs.Count() > 0 && build.BOFFs.SelectMany(xbo => xbo.Abilities).Count() > 0)
                {
                    this.MDW.StartTable();
                    this.MDW.WriteTableHeaders(new string[] { "Bridge Officers", "Power" });
                    this.MDW.WriteTableAlignments(new string[] { "left", "left" });
                    var fst = false;
                    foreach (var boff in build.BOFFs.OrderBy(xbb => xbb.Station.Career).ThenByDescending(xbb => xbb.Rank))
                    {
                        var rw = false;

                        foreach (var ba in boff.Abilities.OrderBy(xba => xba.Rank))
                        {
                            this.MDW.StartTableRow();
                            if (!rw)
                            {
                                if (fst)
                                {
                                    this.MDW.WriteTableCell("");
                                    this.MDW.WriteTableCell("");
                                    this.MDW.EndTableRow();
                                    this.MDW.StartTableRow();
                                }

                                fst = true;
                                rw = true;
                                this.MDW.WriteFormat(tools.GetBOFFDisplayName(boff), true, false, false);
                            }
                            this.MDW.WriteTableCell("");
                            this.MDW.WriteTableCell(ba.DisplayName);
                            this.MDW.EndTableRow();
                        }
                    }
                    this.MDW.EndTable();
                }
                if (build.DOFFs.Where(xdo => xdo.DOFF.Region == DoffRegion.Space).Count() > 0)
                {
                    this.MDW.StartTable();
                    this.MDW.WriteTableHeaders(new string[] { "Duty Officers", "Power" });
                    this.MDW.WriteTableAlignments(new string[] { "left", "left" });
                    foreach (var doff in build.DOFFs.Where(xdo => xdo.DOFF.Region == DoffRegion.Space))
                    {
                        this.MDW.StartTableRow();
                        this.MDW.WriteTableCell(string.Concat(tools.GetEnumDisplayName(doff.Rarity), " ", tools.GetEnumDisplayName(doff.DOFF.Specialization)));
                        this.MDW.WriteTableCell(doff.DOFF.Ability);
                        this.MDW.EndTableRow();
                    }
                    this.MDW.EndTable();
                }
                this.MDW.WriteRule();
            }

            if (build.Traits.Count() > 0)
            {
                this.MDW.WriteHeader("Character, Reputation, and Starship Traits", 2);
                var pstraits = build.Traits.Where(xt => xt.Type == TraitType.PersonalSpace);
                if (pstraits.Count() > 0)
                {
                    this.MDW.StartTable();
                    this.MDW.WriteTableHeaders(new string[] { "Personal Space Traits", "Description" });
                    this.MDW.WriteTableAlignments(new string[] { "left", "left" });
                    foreach (var trait in pstraits)
                    {
                        this.MDW.StartTableRow();
                        this.MDW.WriteTableCell(trait.Name);
                        this.MDW.WriteTableCell(trait.Description);
                        this.MDW.EndTableRow();
                    }
                    this.MDW.EndTable();
                }
                var rstraits = build.Traits.Where(xt => xt.Type == TraitType.ReputationSpace);
                if (rstraits.Count() > 0)
                {
                    this.MDW.StartTable();
                    this.MDW.WriteTableHeaders(new string[] { "Space Reputation Traits", "Description" });
                    this.MDW.WriteTableAlignments(new string[] { "left", "left" });
                    foreach (var trait in rstraits)
                    {
                        this.MDW.StartTableRow();
                        this.MDW.WriteTableCell(trait.Name);
                        this.MDW.WriteTableCell(trait.Description);
                        this.MDW.EndTableRow();
                    }
                    this.MDW.EndTable();
                }
                var ratraits = build.Traits.Where(xt => xt.Type == TraitType.ReputationActive);
                if (ratraits.Count() > 0)
                {
                    this.MDW.StartTable();
                    this.MDW.WriteTableHeaders(new string[] { "Active Reputation Traits", "Description" });
                    this.MDW.WriteTableAlignments(new string[] { "left", "left" });
                    foreach (var trait in ratraits)
                    {
                        this.MDW.StartTableRow();
                        this.MDW.WriteTableCell(trait.Name);
                        this.MDW.WriteTableCell(trait.Description);
                        this.MDW.EndTableRow();
                    }
                    this.MDW.EndTable();
                }
                var straits = build.Traits.Where(xt => xt.Type == TraitType.Starship);
                if (straits.Count() > 0)
                {
                    this.MDW.StartTable();
                    this.MDW.WriteTableHeaders(new string[] { "Starship Traits", "Description" });
                    this.MDW.WriteTableAlignments(new string[] { "left", "left" });
                    foreach (var trait in straits)
                    {
                        this.MDW.StartTableRow();
                        this.MDW.WriteTableCell(trait.Name);
                        this.MDW.WriteTableCell(trait.Description);
                        this.MDW.EndTableRow();
                    }
                    this.MDW.EndTable();
                }
                this.MDW.WriteRule();
            }

            if (!string.IsNullOrWhiteSpace(build.Notes))
            {
                this.MDW.WriteHeader("Build Notes", 2);
                this.MDW.WriteHtml(build.Notes);
                this.MDW.WriteRule();
            }

            this.MDW.WriteParagraph(string.Format("*Above was translated automatically from http://skillplanner.stoacademy.com/{0} using [Automatic Build Converter](https://www.reddit.com/r/stobuilds/comments/5466ul/automatic_sto_academy_build_converter/), version {1}. Questions and problems related to output (but not the build) are to be directed at [Emzi0767](https://www.reddit.com/message/compose/?to=eMZi0767&subject=STO+Academy+Converter).*", build.ID, this.GetAbtVersion()));
        }

        private void WriteBuildGround(StoAcademyBuild build, StoAcademyTools tools)
        {
            this.MDW.WriteHeaderLink(build.Name, build.Url, 1);
            this.MDW.WriteRule();

            var ciths = new string[] { "Category", "Data" };
            var cialn = new string[] { "left", "left" };
            var cidat = new string[][]
            {
                new string[] { "*Captain Career*", tools.GetEnumDisplayName(build.Career) },
                new string[] { "*Captain Faction*", tools.GetEnumDisplayName(build.Faction) },
                new string[] { "*Primary Specialization*", tools.GetEnumDisplayName(build.SpecializationPrimary) },
                new string[] { "*Secondary Specialization*", tools.GetEnumDisplayName(build.SpecializationSecondary) },
            };
            this.MDW.WriteHeader("Captain Info", 2);
            this.MDW.WriteTable(ciths, cialn, cidat);
            this.MDW.WriteRule();

            var skills = this.ReduceSkills(build.Skills).Where(xsk => xsk.Region == SkillRegion.Ground).Select(xsk => new { s = xsk, i = int.Parse(xsk.SkillID.Substring(1)) });
            if (skills.Count() > 0)
            {
                this.MDW.WriteHeader("Skill Tree", 2);
                this.MDW.StartTable();
                this.MDW.WriteTableHeaders(new string[] { "Skill", "Skill" });
                this.MDW.WriteTableAlignments(new string[] { "center", "center" });
                var sl1 = skills.Where(xski => xski.i < 7);
                var sl2 = skills.Where(xski => xski.i > 12 && xski.i < 17);
                var sr1 = skills.Where(xski => xski.i > 6 && xski.i < 13);
                var sr2 = skills.Where(xski => xski.i > 16);
                var max1 = Math.Max(sl1.Count(), sr1.Count());
                var max2 = Math.Max(sl2.Count(), sr2.Count());

                for (int i = 0; i < max1; i++)
                {
                    this.MDW.StartTableRow();

                    var xsl = sl1.ElementAtOrDefault(i);
                    var xsr = sr1.ElementAtOrDefault(i);

                    if (xsl != null)
                        this.MDW.WriteTableCell(xsl.s.Name);
                    else
                        this.MDW.WriteTableCell("");
                    if (xsr != null)
                        this.MDW.WriteTableCell(xsr.s.Name);
                    else
                        this.MDW.WriteTableCell("");

                    this.MDW.EndTableRow();
                }
                    
                this.MDW.StartTableRow();
                this.MDW.WriteTableCell("");
                this.MDW.WriteTableCell("");
                this.MDW.EndTableRow();

                for (int i = 0; i < max2; i++)
                {
                    this.MDW.StartTableRow();

                    var xsl = sl2.ElementAtOrDefault(i);
                    var xsr = sr2.ElementAtOrDefault(i);

                    if (xsl != null)
                        this.MDW.WriteTableCell(xsl.s.Name);
                    else
                        this.MDW.WriteTableCell("");
                    if (xsr != null)
                        this.MDW.WriteTableCell(xsr.s.Name);
                    else
                        this.MDW.WriteTableCell("");

                    this.MDW.EndTableRow();
                }
                this.MDW.EndTable();
                this.MDW.WriteRule();

                var unlocks = build.SkillUnlocks.OrderBy(xsku => xsku.UnlockID);
                if (unlocks.Count() > 0)
                {
                    string[] purchases = { "**1**", "**2**", "**3**", "**4**", "**5**", "**6**", "**7**", "**8**", "**9**", "**10**" };
                    var ug = unlocks != null ? unlocks.Where(xsku => xsku.Career == SkillUnlockCareer.Unknown) : null;
                    this.MDW.WriteHeader("Ground Unlocks", 2);
                    this.MDW.StartTable();
                    this.MDW.WriteTableHeaders(new string[] { "Purchases", "Unlock" });
                    this.MDW.WriteTableAlignments(new string[] { "left", "center" });
                    for (int i = 0; i < purchases.Length; i++)
                    {
                        var xug = ug != null ? ug.ElementAtOrDefault(i) : default(StoAcademySkillUnlock);

                        if (xug.UnlockID == null)
                            break;

                        this.MDW.StartTableRow();
                        this.MDW.WriteTableCell(purchases[i]);
                        this.MDW.WriteTableCell(xug.Name);
                        this.MDW.EndTableRow();
                    }
                    this.MDW.EndTable();
                }
                this.MDW.WriteRule();
            }

            if (!string.IsNullOrWhiteSpace(build.Description))
            {
                this.MDW.WriteHeader("Build Description", 2);
                this.MDW.WriteHtml(build.Description);
                this.MDW.WriteRule();
            }

            var components = build.BuildEquipment.OrderBy(xbi => xbi.ItemType);
            if (components.Count() > 0)
            {
                this.MDW.WriteHeader("Captain Loadout", 2);
                this.MDW.StartTable();
                this.MDW.WriteTableHeaders(new string[] { "Slot", "Component", "Rarity" });
                this.MDW.WriteTableAlignments(new string[] { "left", "left", "left" });
                var ptype = BuildItemType.Unknown;
                foreach (var item in components)
                {
                    this.MDW.StartTableRow();
                    if (item.ItemType != ptype)
                    {
                        if (ptype != BuildItemType.Unknown)
                        {
                            this.MDW.WriteTableCell("");
                            this.MDW.WriteTableCell("");
                            this.MDW.WriteTableCell("");
                            this.MDW.EndTableRow();
                            this.MDW.StartTableRow();
                        }

                        ptype = item.ItemType;
                        this.MDW.WriteFormat(tools.GetEnumDisplayName(item.ItemType), true, false, false);
                    }
                    this.MDW.WriteTableCell("");
                    this.MDW.WriteTableCell(item.Item.FullName);
                    this.MDW.WriteTableCell(tools.GetEnumDisplayName(item.Item.Rarity));
                    this.MDW.EndTableRow();
                }
                this.MDW.EndTable();
                this.MDW.WriteRule();
            }

            if ((build.AwayTeam.Count() > 0 && build.AwayTeam.SelectMany(xbo => xbo.Abilities).Count() > 0) || build.DOFFs.Where(xdo => xdo.DOFF.Region == DoffRegion.Ground).Count() > 0)
            {
                this.MDW.WriteHeader("Officers and Crew", 2);
                if (build.AwayTeam.Count() > 0 && build.AwayTeam.SelectMany(xbo => xbo.Abilities).Count() > 0)
                {
                    this.MDW.StartTable();
                    this.MDW.WriteTableHeaders(new string[] { "Away Team", "Power" });
                    this.MDW.WriteTableAlignments(new string[] { "left", "left" });
                    var fst = false;
                    foreach (var boff in build.AwayTeam.OrderBy(xbb => xbb.Station.Career).ThenByDescending(xbb => xbb.Rank))
                    {
                        var rw = false;

                        foreach (var ba in boff.Abilities.OrderBy(xba => xba.Rank))
                        {
                            this.MDW.StartTableRow();
                            if (!rw)
                            {
                                if (fst)
                                {
                                    this.MDW.WriteTableCell("");
                                    this.MDW.WriteTableCell("");
                                    this.MDW.EndTableRow();
                                    this.MDW.StartTableRow();
                                }

                                fst = true;
                                rw = true;
                                this.MDW.WriteFormat(tools.GetBOFFDisplayName(boff), true, false, false);
                            }
                            this.MDW.WriteTableCell("");
                            this.MDW.WriteTableCell(ba.DisplayName);
                            this.MDW.EndTableRow();
                        }
                    }
                    this.MDW.EndTable();
                }
                if (build.DOFFs.Where(xdo => xdo.DOFF.Region == DoffRegion.Ground).Count() > 0)
                {
                    this.MDW.StartTable();
                    this.MDW.WriteTableHeaders(new string[] { "Duty Officers", "Power" });
                    this.MDW.WriteTableAlignments(new string[] { "left", "left" });
                    foreach (var doff in build.DOFFs.Where(xdo => xdo.DOFF.Region == DoffRegion.Ground))
                    {
                        this.MDW.StartTableRow();
                        this.MDW.WriteTableCell(string.Concat(tools.GetEnumDisplayName(doff.Rarity), " ", tools.GetEnumDisplayName(doff.DOFF.Specialization)));
                        this.MDW.WriteTableCell(doff.DOFF.Ability);
                        this.MDW.EndTableRow();
                    }
                    this.MDW.EndTable();
                }
                this.MDW.WriteRule();
            }

            if (build.Traits.Count() > 0)
            {
                this.MDW.WriteHeader("Character and Reputation Traits", 2);
                var pgtraits = build.Traits.Where(xt => xt.Type == TraitType.PersonalGround);
                if (pgtraits.Count() > 0)
                {
                    this.MDW.StartTable();
                    this.MDW.WriteTableHeaders(new string[] { "Personal Ground Traits", "Description" });
                    this.MDW.WriteTableAlignments(new string[] { "left", "left" });
                    foreach (var trait in pgtraits)
                    {
                        this.MDW.StartTableRow();
                        this.MDW.WriteTableCell(trait.Name);
                        this.MDW.WriteTableCell(trait.Description);
                        this.MDW.EndTableRow();
                    }
                    this.MDW.EndTable();
                }
                var rgtraits = build.Traits.Where(xt => xt.Type == TraitType.ReputationGround);
                if (rgtraits.Count() > 0)
                {
                    this.MDW.StartTable();
                    this.MDW.WriteTableHeaders(new string[] { "Ground Reputation Traits", "Description" });
                    this.MDW.WriteTableAlignments(new string[] { "left", "left" });
                    foreach (var trait in rgtraits)
                    {
                        this.MDW.StartTableRow();
                        this.MDW.WriteTableCell(trait.Name);
                        this.MDW.WriteTableCell(trait.Description);
                        this.MDW.EndTableRow();
                    }
                    this.MDW.EndTable();
                }
                var ratraits = build.Traits.Where(xt => xt.Type == TraitType.ReputationActive);
                if (ratraits.Count() > 0)
                {
                    this.MDW.StartTable();
                    this.MDW.WriteTableHeaders(new string[] { "Active Reputation Traits", "Description" });
                    this.MDW.WriteTableAlignments(new string[] { "left", "left" });
                    foreach (var trait in ratraits)
                    {
                        this.MDW.StartTableRow();
                        this.MDW.WriteTableCell(trait.Name);
                        this.MDW.WriteTableCell(trait.Description);
                        this.MDW.EndTableRow();
                    }
                    this.MDW.EndTable();
                }
                this.MDW.WriteRule();
            }

            if (!string.IsNullOrWhiteSpace(build.Notes))
            {
                this.MDW.WriteHeader("Build Notes", 2);
                this.MDW.WriteHtml(build.Notes);
                this.MDW.WriteRule();
            }

            this.MDW.WriteParagraph(string.Format("*Above was translated automatically from http://skillplanner.stoacademy.com/{0} using [Automatic Build Converter](https://www.reddit.com/r/stobuilds/comments/5466ul/automatic_sto_academy_build_converter/), version {1}. Questions and problems related to output (but not the build) are to be directed at [Emzi0767](https://www.reddit.com/message/compose/?to=eMZi0767&subject=STO+Academy+Converter).*", build.ID, this.GetAbtVersion()));
        }

        private IEnumerable<StoAcademySkill> ReduceSkills(IEnumerable<StoAcademySkill> skill_tree)
        {
            var csk = skill_tree.GroupBy(xsk => xsk.Rank);
            var sk = new List<StoAcademySkill>();

            foreach (var xcsk in csk)
            {
                var rsk = xcsk.GroupBy(xsk => xsk.Career);
                foreach (var xrsk in rsk)
                {
                    var nsk = xrsk.GroupBy(xsk => xsk.Name.Replace("Advanced ", "").Replace("Improved ", "").Replace(" Expert", "").Replace(" Master", "").Replace(" Proficiency", ""));
                    foreach (var xnsk in nsk)
                    {
                        sk.Add(xnsk.OrderByDescending(xsk => xsk.SkillID).FirstOrDefault());
                    }
                }
            }

            return sk.OrderBy(xsk => xsk.Rank).ThenBy(xsk => xsk.SkillID);
        }

        private Version GetAbtVersion()
        {
            var a = Assembly.GetEntryAssembly();
            var n = a.GetName();
            return n.Version;
        }

        public void Dispose()
        {
            this.MDW.Dispose();
        }
    }
}
