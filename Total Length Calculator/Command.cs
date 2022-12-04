#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
#endregion

namespace Total_Length_Calculator
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        double ductsLength;
        double pipeLength;
        double cableTraysLength;
        double conduitsLength;
        const double factorToMeter = 0.3048;

        public Result Execute(
                      ExternalCommandData commandData,
                      ref string message,
                      ElementSet elements)
        {
            try
            {
                UIApplication uiapp = commandData.Application;
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Application app = uiapp.Application;
                Document doc = uidoc.Document;

                var SelectedElements = SelectElements(uidoc, doc);
                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("Transaction Name");
                    ductsLength = 0;
                    pipeLength = 0;
                    cableTraysLength = 0;
                    conduitsLength = 0;
                    foreach (Element elem in SelectedElements)
                    {
                        if (elem is Duct)
                        {
                            Parameter length = elem.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                            ductsLength += Math.Round(length.AsDouble() * factorToMeter, 2);
                        }
                        if (elem is CableTray)
                        {
                            Parameter length = elem.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                            cableTraysLength += Math.Round(length.AsDouble() * factorToMeter, 2);
                        }
                        if (elem is Pipe)
                        {
                            Parameter length = elem.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                            pipeLength += Math.Round(length.AsDouble() * factorToMeter, 2);
                        }
                        if (elem is Conduit)
                        {
                            Parameter length = elem.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                            conduitsLength += Math.Round(length.AsDouble() * factorToMeter, 2);
                        }

                    }
                    TaskDialog.Show("Succeed!!!",
                        "Total length of ducts: " + ductsLength.ToString() + "[m]\n" +
                        "Total length of pipes: " + pipeLength.ToString() + "[m]\n" +
                        "Total length of cable trays: " + cableTraysLength.ToString() + "[m]\n" +
                        "Total length of conduits: " + conduitsLength.ToString() + "[m]\n" +
                        "Total length of selected elements: " + (ductsLength + pipeLength + cableTraysLength + conduitsLength).ToString() + "[m]");
                    tx.Commit();
                }
            }
            catch (Exception)
            {

                TaskDialog.Show("Error!", "Oops, something went wrong...\nPlease try again.");
            }


            return Result.Succeeded;
        }
        public List<Element> SelectElements(UIDocument uidoc, Document doc)
            {
                var reference = uidoc.Selection.PickObjects(ObjectType.Element);
                List<Element> listOfReference = new List<Element>();
                foreach (var item in reference)
                {
                    listOfReference.Add(uidoc.Document.GetElement(item));
                }
                return listOfReference;
            }
        public string GetParameterValue(Parameter parameter)
        {
            switch (parameter.StorageType)
            {
                case StorageType.Double:
                    return parameter.AsValueString();
                case StorageType.ElementId:
                    return parameter.AsElementId().ToString();
                case StorageType.Integer:
                    return parameter.AsValueString();
                case StorageType.None:
                    return parameter.AsValueString();
                case StorageType.String:
                    return parameter.AsString();
                default:
                    return "";
            }
        }


            
        
    }
}
