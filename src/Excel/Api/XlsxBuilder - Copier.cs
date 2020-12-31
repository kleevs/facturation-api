////using FacturationApi.Models;
////using FacturationApi.Spi;
////using System;
////using System.Collections.Generic;
////using System.IO;
////using System.IO.Compression;
////using System.Linq;
////using System.Text;
////using System.Xml;

////namespace Excel
////{
////    public class XlsxBuilder
////    {
////        private readonly Xlsx _xlsxFile;

////        public XlsxBuilder(Xlsx xlsxFile)
////        {
////            _xlsxFile = xlsxFile;
////        }

////        public byte[] Build()
////        {
////            using (var archiveStream = new MemoryStream())
////            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
////            {
////                var numSheet = _xlsxFile.Sheets.Count();

////                using (var contentTypeStream = archive.CreateEntry("[Content_Types].xml", CompressionLevel.Fastest).Open()) 
////                {
////                    var xmlBuilder = new XmlBuilder();
////                    xmlBuilder.SetDeclaration("1.0", "UTF-8", true);

////                    var types = xmlBuilder.Append("Types", "http://schemas.openxmlformats.org/package/2006/content-types");
////                    types.Append("Default", null, "ContentType", "application/vnd.openxmlformats-package.relationships+xml", "Extension", "rels");
////                    types.Append("Default", null, "ContentType", "application/xml", "Extension", "xml");
////                    types.Append("Override", null, "ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", "PartName", "/xl/workbook.xml");
////                    ////types.Append("Override", null, "ContentType", "application/vnd.openxmlformats-officedocument.theme+xml", "PartName", "/xl/theme/theme1.xml");
////                    ////types.Append("Override", null, "ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml", "PartName", "/xl/styles.xml");
////                    types.Append("Override", null, "ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.sharedStrings+xml", "PartName", "/xl/sharedStrings.xml");
////                    ////types.Append("Override", null, "ContentType", "application/vnd.openxmlformats-package.core-properties+xml", "PartName", "/docProps/core.xml");
////                    ////types.Append("Override", null, "ContentType", "application/vnd.openxmlformats-officedocument.extended-properties+xml", "PartName", "/docProps/app.xml");

////                    for (var i = 1; i <= _xlsxFile.Sheets.Count(); i++) 
////                    {
////                        types.Append("Override", null, "ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml", "PartName", $"/xl/worksheets/sheet{i}.xml");
////                    }

////                    xmlBuilder.ToBuild(contentTypeStream);
////                }

////                ////using (var contentTypeStream = archive.CreateEntry("docProps/app.xml", CompressionLevel.Fastest).Open())
////                ////{
////                ////    var xmlBuilder = new XmlBuilder();
////                ////    xmlBuilder.SetDeclaration("1.0", "UTF-8", true);

////                ////    var properties = xmlBuilder.Append("Properties", "http://schemas.openxmlformats.org/officeDocument/2006/extended-properties", 
////                ////        "xlmns:vt", "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes");
////                ////    properties.Append("Application", null).Append("Microsoft Excel");
////                ////    properties.Append("DocSecurity", null).Append("0");
////                ////    properties.Append("ScaleCrop", null).Append("false");

////                ////    xmlBuilder.ToBuild(contentTypeStream);
////                ////}

////                using (var contentTypeStream = archive.CreateEntry("_rels/.rels", CompressionLevel.Fastest).Open())
////                {
////                    var xmlBuilder = new XmlBuilder();
////                    xmlBuilder.SetDeclaration("1.0", "UTF-8", true);

////                    var relationShip = xmlBuilder.Append("Relationships", "http://schemas.openxmlformats.org/package/2006/relationships");                       
////                    //relationShip.Append("Relationship", null, "Id", "rId2", "Target", "docProps/app.xml", "Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties");
////                    relationShip.Append("Relationship", null, "Id", "rId1", "Target", "xl/workbook.xml", "Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument");

////                    xmlBuilder.ToBuild(contentTypeStream);
////                }

////                using (var contentTypeStream = archive.CreateEntry("xl/workbook.xml", CompressionLevel.Fastest).Open())
////                {
////                    var xmlBuilder = new XmlBuilder();
////                    xmlBuilder.SetDeclaration("1.0", "UTF-8", true);

////                    var workbook = xmlBuilder.Append("workbook", "http://schemas.openxmlformats.org/spreadsheetml/2006/main", 
////                        "xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", 
////                        "xmlns:mc", "http://schemas.openxmlformats.org/markup-compatibility/2006", 
////                        "mc:Ignorable", "x15 xr xr6 xr10", 
////                        "xmlns:x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main", 
////                        "xmlns:xr", "http://schemas.microsoft.com/office/spreadsheetml/2014/revision", 
////                        "xmlns:xr6", "http://schemas.microsoft.com/office/spreadsheetml/2016/revision6", 
////                        "xmlns:xr10", "http://schemas.microsoft.com/office/spreadsheetml/2016/revision10");
////                    workbook.Append("fileVersion", null, "appName", "xl", "lastEdited", "7", "lowestEdited", "7", "rupBuild", "22827");
////                    workbook.Append("workbookPr", null, "defaultThemeVersion", "166925");
////                    var alternateContent = workbook.Append("mc:AlternateContent", null, "xmlns:mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
////                    var choice = alternateContent.Append("mc:Choice", null, "Requires", "x15");
////                    choice.Append("x15ac:absPath", null, "url", "C:/_Projects/Azure/facturation-api/", "xmlns:x15ac", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/ac");
////                    workbook.Append("xr:revisionPtr", null, "revIDLastSave", "0", "documentId", "13_ncr:40001_{EA8B1FA7-F871-4792-9C97-2B70B784B7AA}", "xr6:coauthVersionLast", "45", "xr6:coauthVersionMax", "45", "xr10:uidLastSave", "{00000000-0000-0000-0000-000000000000}");
////                    var bookViews = workbook.Append("bookViews", null);
////                    bookViews.Append("workbookView", null, "xWindow", "-110", "yWindow", "-110", "windowWidth", "21820", "windowHeight", "14020");
////                    var sheets = workbook.Append("sheets", null);

////                    for (var i = 1; i <= numSheet; i++)
////                    {
////                        sheets.Append("sheet", null, "name", $"{_xlsxFile.Sheets.ElementAt(i-1).Name}", "sheetId", $"{i}", "r:id", $"rId{i}");
////                    }

////                    workbook.Append("calcPr", null, "calcId", "191029");
////                    var extLst = workbook.Append("extLst", null);

////                    var ext = extLst.Append("ext", null, "uri", "{140A7094-0E35-4892-8432-C4D2E57EDEB5}", "xmlns:x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main");
////                    ext.Append("x15:workbookPr", null, "chartTrackingRefBase", "1");
////                    ext = extLst.Append("ext", null, "uri", "{B58B0392-4F1F-4190-BB64-5DF3571DCE5F}", "xmlns:xcalcf", "http://schemas.microsoft.com/office/spreadsheetml/2018/calcfeatures");
////                    var calcFeatures = ext.Append("xcalcf:calcFeatures", null);
////                    calcFeatures.Append("xcalcf:feature", null, "name", "microsoft.com:RD");
////                    calcFeatures.Append("xcalcf:feature", null, "name", "microsoft.com:Single");
////                    calcFeatures.Append("xcalcf:feature", null, "name", "microsoft.com:FV");
////                    calcFeatures.Append("xcalcf:feature", null, "name", "microsoft.com:CNMTM");

////                    xmlBuilder.ToBuild(contentTypeStream);
////                }

////                using (var contentTypeStream = archive.CreateEntry("xl/_rels/workbook.xml.rels", CompressionLevel.Fastest).Open())
////                {
////                    var xmlBuilder = new XmlBuilder();
////                    xmlBuilder.SetDeclaration("1.0", "UTF-8", true);

////                    var relationShips = xmlBuilder.Append("Relationships", "http://schemas.openxmlformats.org/package/2006/relationships");
////                    ////relationShips.Append("Relationship", null, "Target", "styles.xml", "Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles", "Id", $"rId{numSheet + 2}");
////                    relationShips.Append("Relationship", null, "Target", "sharedStrings.xml", "Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/sharedStrings", "Id", $"rId{numSheet + 3}");
////                    ////relationShips.Append("Relationship", null, "Target", "theme/theme1.xml", "Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/theme", "Id", $"rId{numSheet + 1 }");

////                    for (var i = 1; i <= numSheet; i++)
////                    {
////                        relationShips.Append("Relationship", null, "Target", $"worksheets/sheet{i}.xml", "Type", "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet", "Id", $"rId{i}");
////                    }

////                    xmlBuilder.ToBuild(contentTypeStream);
////                }

////                ////using (var contentTypeStream = archive.CreateEntry("xl/styles.xml", CompressionLevel.Fastest).Open())
////                ////{
////                ////    var xmlBuilder = new XmlBuilder();
////                ////    xmlBuilder.SetDeclaration("1.0", "UTF-8", true);

////                ////    var styleSheet = xmlBuilder.Append("styleSheet", "http://schemas.openxmlformats.org/spreadsheetml/2006/main", 
////                ////        "xmlns:x16r2", "http://schemas.microsoft.com/office/spreadsheetml/2015/02/main", 
////                ////        "xmlns:x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac", 
////                ////        "mc:Ignorable", "x14ac x16r2", 
////                ////        "xmlns:mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");

////                ////    xmlBuilder.ToBuild(contentTypeStream);
////                ////}

////                using (var contentTypeStream = archive.CreateEntry("xl/sharedStrings.xml", CompressionLevel.Fastest).Open())
////                {
////                    var xmlBuilder = new XmlBuilder();
////                    xmlBuilder.SetDeclaration("1.0", "UTF-8", true);

////                    var sharedStrings = xmlBuilder.Append("sst", "http://schemas.openxmlformats.org/spreadsheetml/2006/main", "uniqueCount", "1", "count", "1");
////                    sharedStrings.Append("si", null).Append("t", null).Append("texte");

////                    xmlBuilder.ToBuild(contentTypeStream);
////                }

////                ////using (var contentTypeStream = archive.CreateEntry("xl/theme/theme1.xml", CompressionLevel.Fastest).Open())
////                ////{
////                ////    var xmlBuilder = new XmlBuilder();
////                ////    xmlBuilder.SetDeclaration("1.0", "UTF-8", true);

////                ////    var theme = xmlBuilder.Append("a:theme", "http://schemas.openxmlformats.org/drawingml/2006/main", "name", "Thème Office");

////                ////    xmlBuilder.ToBuild(contentTypeStream);
////                ////}

////                for (var i = 1; i <= numSheet; i++)
////                {
////                    using (var contentTypeStream = archive.CreateEntry($"xl/worksheets/sheet{i}.xml", CompressionLevel.Fastest).Open())
////                    {
////                        var xmlBuilder = new XmlBuilder();
////                        xmlBuilder.SetDeclaration("1.0", "UTF-8", true);

////                        var worksheet = xmlBuilder.Append("worksheet", "http://schemas.openxmlformats.org/spreadsheetml/2006/main",
////                            "xmlns:x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac", 
////                            "mc:Ignorable", "x14ac", 
////                            "xmlns:mc", "http://schemas.openxmlformats.org/markup-compatibility/2006", 
////                            "xmlns:r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
////                        worksheet.Append("dimension", null, "ref", "A1:A1");
////                        var sheetViews = worksheet.Append("sheetViews", null);

////                        sheetViews.Append("sheetView", null, "workbookViewId", "0", "tabSelected", "1")
////                            .Append("selection", null, "sqref", "A1", "activeCell", "A1");
////                        var sheetData = worksheet.Append("sheetData", null);
////                        sheetData.Append("row", null, "r", "1", "x14ac:dyDescent", "0.35", "spans", "1:1")
////                            .Append("c", null, "r", "A1", "t", "s")
////                            .Append("v", null)
////                            .Append("0");

////                        xmlBuilder.ToBuild(contentTypeStream);
////                    }
////                }

////                return archiveStream.ToArray();
////            }
////        }
////    }
////}
