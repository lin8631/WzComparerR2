﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WzComparerR2.WzLib;

namespace WzComparerR2.MapRender.Patches2
{
    class ParticleItem : SceneItem
    {
        public string ParticleName { get; set; }
        public int Rx { get; set; }
        public int Ry { get; set; }
        public int Z { get; set; }
        public SubParticleItem[] SubItems { get; set; }
        public List<QuestInfo> Quest { get; private set; } = new List<QuestInfo>();
        public ItemView View { get; set; }

        public static ParticleItem LoadFromNode(Wz_Node node)
        {
            var item = new ParticleItem()
            {
                ParticleName = node.Text,
                Rx = node.Nodes["rx"].GetValueEx(-100),
                Ry = node.Nodes["ry"].GetValueEx(-100),
                Z = node.Nodes["z"].GetValueEx(0)
            };

            if (node.Nodes["quest"] != null)
            {
                foreach (Wz_Node questNode in node.Nodes["quest"].Nodes)
                {
                    if (int.TryParse(questNode.Text, out int questID))
                    {
                        item.Quest.Add(new QuestInfo(questID, Convert.ToInt32(questNode.Value)));
                    }
                }
            }

            var subItems = new List<SubParticleItem>();
            for (int i = 0; ; i++)
            {
                var subNode = node.Nodes[i.ToString()];
                if (subNode == null)
                {
                    break;
                }
                var subitem = new SubParticleItem()
                {
                    X = subNode.Nodes["x"].GetValueEx(0),
                    Y = subNode.Nodes["y"].GetValueEx(0),
                };

                if (subNode.Nodes["quest"] != null)
                {
                    foreach (Wz_Node questNode in subNode.Nodes["quest"].Nodes)
                    {
                        if (int.TryParse(questNode.Text, out int questID))
                        {
                            subitem.Quest.Add(new QuestInfo(questID, Convert.ToInt32(questNode.Value)));
                        }
                    }
                }
                subItems.Add(subitem);
            }

            if (subItems.Count <= 0)
            {
                subItems.Add(new SubParticleItem()
                {
                    X = node.Nodes["x"].GetValueEx(0),
                    Y = node.Nodes["y"].GetValueEx(0),
                });
            }
            item.SubItems = subItems.ToArray();
            return item;
        }

        public class SubParticleItem
        {
            public int X { get; set; }
            public int Y { get; set; }
            public List<QuestInfo> Quest { get; private set; } = new List<QuestInfo>();
        }

        public class ItemView
        {
            public ParticleSystem ParticleSystem { get; set; }
        }
    }
}
