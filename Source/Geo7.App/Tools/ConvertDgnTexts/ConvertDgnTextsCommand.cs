﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


#if AutoCAD
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
#endif

#if BricsCAD
using Teigha.Runtime;
using Teigha.DatabaseServices;
using Teigha.Geometry;
using Bricscad.ApplicationServices;
using Bricscad.EditorInput;
#endif

namespace Geo7.Tools
{
    public class ConvertDgnTextsCommand : AcCommand
    {
        public ConvertDgnTextsCommand()
        {
            this.DisplayName = AcConsts.ConvertDgnTexts;
            this.Description = AcConsts.ConvertDgnTextsDsc;
            this.SmallImage = null;// Resources.ConvertDgnTxt;
        }

        protected override void ExecuteCore()
        {
            using (var trans = Ac.StartTransaction())
            {
                var txtEnts = trans.GetAllEntities<DBText>();
                Ac.InitProgress(AcConsts.ProcessingData, txtEnts.Count());
                int txtChanged = 0;
                foreach (var tEnt in txtEnts)
                {
                    var acText = ConvertDgnText(tEnt.TextString);
                    if (acText != tEnt.TextString)
                    {
                        tEnt.UpgradeOpen();
                        tEnt.TextString = acText;
                        txtChanged++;
                        Ac.ProgressStep();
                    }
                }
                trans.Commit();
                Ac.WriteLn(AcConsts.XEntitesUpdated, txtChanged);
            }
        }

        private string ConvertDgnText(string dgnText)
        {
            dgnText = dgnText.Replace("%%234", "ę");
            dgnText = dgnText.Replace("%%202", "Ę");
            dgnText = dgnText.Replace("%%243", "ó");
            dgnText = dgnText.Replace("%%211", "Ó");
            dgnText = dgnText.Replace("%%185", "ą");
            dgnText = dgnText.Replace("%%165", "Ą");
            dgnText = dgnText.Replace("%%156", "ś");
            dgnText = dgnText.Replace("%%140", "Ś");
            dgnText = dgnText.Replace("%%179", "ł");
            dgnText = dgnText.Replace("%%163", "Ł");
            dgnText = dgnText.Replace("%%191", "ż");
            dgnText = dgnText.Replace("%%175", "Ż");
            dgnText = dgnText.Replace("%%159", "ź");
            dgnText = dgnText.Replace("%%143", "Ź");
            dgnText = dgnText.Replace("%%230", "ć");
            dgnText = dgnText.Replace("%%198", "Ć");
            dgnText = dgnText.Replace("%%241", "ń");
            dgnText = dgnText.Replace("%%209", "Ń");

            //dgnText = dgnText.Replace("³", "ł");
            dgnText = dgnText.Replace("¿", "ż");
            dgnText = dgnText.Replace("ê", "ę");
            dgnText = dgnText.Replace("ñ", "ń");
            dgnText = dgnText.Replace("³", "ł");
            dgnText = dgnText.Replace("¹", "ą");
            dgnText = dgnText.Replace("¥", "Ą");
            dgnText = dgnText.Replace("¯", "Ż");
            dgnText = dgnText.Replace("£", "Ł");
            dgnText = dgnText.Replace("Æ", "Ć");
            dgnText = dgnText.Replace("Ê", "Ę");
            dgnText = dgnText.Replace("Ñ", "Ń");




            return dgnText;
        }
    }
}
