using Excel.Template.Recette;
using FacturationApi.Models;
using FacturationApi.Spi;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml;

namespace Excel
{
    public class XlsxBuilder
    {
        private readonly Xlsx _xlsxFile;

        public XlsxBuilder(Xlsx xlsxFile)
        {
            _xlsxFile = xlsxFile;
        }

        public byte[] Build()
        {
            using (var zipBuilder = new ZipBuilder())
            {
                zipBuilder.AppendFile("[Content_Types].xml", new Content_Types_xml().TransformText());
                zipBuilder.AppendFile("_rels/.rels", new Template.Recette._rels.rels().TransformText());
                zipBuilder.AppendFile("docProps/app.xml", new Template.Recette.docProps.app_xml().TransformText());
                zipBuilder.AppendFile("docProps/core.xml", new Template.Recette.docProps.core_xml().TransformText());
                zipBuilder.AppendFile("xl/_rels/workbook.xml.rels", new Template.Recette.xl._rels.workbook_xml_rels().TransformText());
                zipBuilder.AppendFile("xl/theme/theme1.xml", new Template.Recette.xl.theme.theme1_xml().TransformText());

                zipBuilder.AppendFile("xl/worksheets/sheet1.xml", new Template.Recette.xl.worksheets.sheet1_xml().TransformText());
                zipBuilder.AppendFile("xl/worksheets/sheet2.xml", new Template.Recette.xl.worksheets.sheet2_xml().TransformText());
                zipBuilder.AppendFile("xl/worksheets/sheet3.xml", new Template.Recette.xl.worksheets.sheet3_xml().TransformText());
                zipBuilder.AppendFile("xl/worksheets/sheet4.xml", new Template.Recette.xl.worksheets.sheet4_xml().TransformText());
                zipBuilder.AppendFile("xl/worksheets/sheet5.xml", new Template.Recette.xl.worksheets.sheet5_xml().TransformText());

                zipBuilder.AppendFile("xl/calcChain.xml", new Template.Recette.xl.calcChain_xml().TransformText());
                zipBuilder.AppendFile("xl/sharedStrings.xml", new Template.Recette.xl.sharedStrings_xml().TransformText());
                zipBuilder.AppendFile("xl/styles.xml", new Template.Recette.xl.styles_xml().TransformText());
                zipBuilder.AppendFile("xl/workbook.xml", new Template.Recette.xl.workbook_xml().TransformText());

                var logFile = System.IO.File.Create("C:/_Projects/test.xlsx");
                var buffer = zipBuilder.Build();
                logFile.Write(buffer, 0, buffer.Length);
                logFile.Dispose();

                return zipBuilder.Build();
            }
        }
    }
}
