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
using System.IO;
#endif

namespace Geo7.Tools
{
    public class TextExpTextCommand : AcCommand
    {
        public TextExpTextCommand()
        {
            this.DisplayName = AcConsts.ExportTexts;
            this.Description = AcConsts.ExportTextsDsc;
            //this.SmallImage = Geo7.Properties.Resources.ExportTxt;
        }

        protected override void ExecuteCore()
        {
            var storage = AppServices.SaveFileDialog.ShowTextLinesWritersDialog(Ac.GetLastFileName("points"));
            if (storage == null)
                return;


            using (storage)
            {
                using (var lTrans = Ac.StartTransaction())
                {
                    var allTexts = lTrans.GetAllEntities<DBText>().Where(ent => ent.Visible).Where(ent => !ent.TextString.IsEmpty());

                    Ac.InitProgress(AppServices.Strings.Saving, allTexts.Count());
                    foreach (var textEnt in allTexts)
                    {
                        if (lTrans.IsDisplayed(textEnt))
                        {
                            var x = textEnt.Position.X.ToString(XYCoordinate.DefaultPrecision);
                            var y = textEnt.Position.Y.ToString(XYCoordinate.DefaultPrecision);
                            var z = textEnt.Position.Z.ToString(XYCoordinate.DefaultPrecision);
                            storage.WriteFields(textEnt.TextString, x, y, z);
                        }
                        Ac.ProgressStep();
                    }
                    Ac.WriteLn(AppServices.Strings.XPointsSaved, storage.LinesWritten);
                }
            }
            
        }
    }
}
