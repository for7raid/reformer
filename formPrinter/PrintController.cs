using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using formPrinter.Model;
using System.Windows.Controls;

namespace formPrinter
{
    public class PrintController
    {
        public void Print(Form form)
        {
            var pd = new PrintDialog();
            if (pd.ShowDialog() != true) return;
            

            
        }

        public void GenerateFixedDocuemnt()
        {
            FixedDocument document = new FixedDocument();
            //document.DocumentPaginator.PageSize = new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);
        }
    }
}
