namespace ReportLibrary
{
    partial class M1001
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.TypeReportSource typeReportSource1 = new Telerik.Reporting.TypeReportSource();
            Telerik.Reporting.TypeReportSource typeReportSource2 = new Telerik.Reporting.TypeReportSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(M1001));
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter4 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter5 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter6 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter7 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter8 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter9 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter10 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter11 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter12 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter13 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter14 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter15 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter16 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.RenderingSettings renderingSettings1 = new Telerik.Reporting.RenderingSettings();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.DescendantSelector descendantSelector1 = new Telerik.Reporting.Drawing.DescendantSelector();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.DescendantSelector descendantSelector2 = new Telerik.Reporting.Drawing.DescendantSelector();
            this.groupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.groupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.detailSection1 = new Telerik.Reporting.DetailSection();
            this.reportHeaderSection1 = new Telerik.Reporting.ReportHeaderSection();
            this.TahsilMakbuzu1 = new Telerik.Reporting.SubReport();
            this.TahsilMakbuzu2 = new Telerik.Reporting.SubReport();
            this.sqlDataSource2 = new Telerik.Reporting.SqlDataSource();
            this.sqlDataSource1 = new Telerik.Reporting.SqlDataSource();
            this.sqlDataSource4 = new Telerik.Reporting.SqlDataSource();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // groupFooterSection
            // 
            this.groupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.132D);
            this.groupFooterSection.Name = "groupFooterSection";
            // 
            // groupHeaderSection
            // 
            this.groupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.132D);
            this.groupHeaderSection.Name = "groupHeaderSection";
            // 
            // detailSection1
            // 
            this.detailSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.132D);
            this.detailSection1.Name = "detailSection1";
            this.detailSection1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.detailSection1.Style.Font.Bold = true;
            this.detailSection1.Style.Visible = false;
            // 
            // reportHeaderSection1
            // 
            this.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(2.6D);
            this.reportHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.TahsilMakbuzu1,
            this.TahsilMakbuzu2});
            this.reportHeaderSection1.Name = "reportHeaderSection1";
            // 
            // TahsilMakbuzu1
            // 
            this.TahsilMakbuzu1.Bindings.Add(new Telerik.Reporting.Binding("Visible", "= Parameters.isReceipt1.Value"));
            this.TahsilMakbuzu1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.2D));
            this.TahsilMakbuzu1.Name = "TahsilMakbuzu1";
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("studentID", "= Parameters.studentID.Value"));
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("language", "= Parameters.language.Value"));
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("period", "= Parameters.period.Value"));
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("schoolID", "= Parameters.schoolID.Value"));
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("userID", "= Parameters.userID.Value"));
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("receiptNo", "= Parameters.receiptNo.Value"));
            typeReportSource1.TypeName = "ReportLibrary.M1011Receipt, ReportLibrary, Version=15.2.21.1125, Culture=neutral," +
    " PublicKeyToken=null";
            this.TahsilMakbuzu1.ReportSource = typeReportSource1;
            this.TahsilMakbuzu1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(19.2D), Telerik.Reporting.Drawing.Unit.Cm(1.168D));
            // 
            // TahsilMakbuzu2
            // 
            this.TahsilMakbuzu2.Bindings.Add(new Telerik.Reporting.Binding("Visible", "= IIf(Fields.PrintQuantity = 1 or Parameters.isReceipt1.Value = 0, False, True)"));
            this.TahsilMakbuzu2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(1.4D));
            this.TahsilMakbuzu2.Name = "TahsilMakbuzu2";
            typeReportSource2.Parameters.Add(new Telerik.Reporting.Parameter("studentID", "= Parameters.studentID.Value"));
            typeReportSource2.Parameters.Add(new Telerik.Reporting.Parameter("language", "= Parameters.language.Value"));
            typeReportSource2.Parameters.Add(new Telerik.Reporting.Parameter("period", "= Parameters.period.Value"));
            typeReportSource2.Parameters.Add(new Telerik.Reporting.Parameter("schoolID", "= Parameters.schoolID.Value"));
            typeReportSource2.Parameters.Add(new Telerik.Reporting.Parameter("userID", "= Parameters.userID.Value"));
            typeReportSource2.Parameters.Add(new Telerik.Reporting.Parameter("receiptNo", "= Parameters.receiptNo.Value"));
            typeReportSource2.TypeName = "ReportLibrary.M1011Receipt, ReportLibrary, Version=15.2.21.1125, Culture=neutral," +
    " PublicKeyToken=null";
            this.TahsilMakbuzu2.ReportSource = typeReportSource2;
            this.TahsilMakbuzu2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(19.2D), Telerik.Reporting.Drawing.Unit.Cm(1.168D));
            // 
            // sqlDataSource2
            // 
            this.sqlDataSource2.ConnectionString = "Data Source=.;Initial Catalog=10001;Integrated Security=True";
            this.sqlDataSource2.Name = "sqlDataSource2";
            this.sqlDataSource2.Parameters.Add(new Telerik.Reporting.SqlDataSourceParameter("@userID", System.Data.DbType.Int32, "= Parameters.userID.Value"));
            this.sqlDataSource2.Parameters.Add(new Telerik.Reporting.SqlDataSourceParameter("@schoolID", System.Data.DbType.Int32, "= Parameters.schoolID.Value"));
            this.sqlDataSource2.ProviderName = "System.Data.SqlClient";
            this.sqlDataSource2.SelectCommand = resources.GetString("sqlDataSource2.SelectCommand");
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionString = "Data Source=.;Initial Catalog=10001;Integrated Security=True";
            this.sqlDataSource1.Name = "sqlDataSource1";
            this.sqlDataSource1.Parameters.Add(new Telerik.Reporting.SqlDataSourceParameter("@period", System.Data.DbType.String, "= Parameters.period.Value"));
            this.sqlDataSource1.Parameters.Add(new Telerik.Reporting.SqlDataSourceParameter("@schoolID", System.Data.DbType.Int32, "= Parameters.schoolID.Value"));
            this.sqlDataSource1.Parameters.Add(new Telerik.Reporting.SqlDataSourceParameter("@studentID", System.Data.DbType.Int32, "= Parameters.studentID.Value"));
            this.sqlDataSource1.ProviderName = "System.Data.SqlClient";
            this.sqlDataSource1.SelectCommand = resources.GetString("sqlDataSource1.SelectCommand");
            // 
            // sqlDataSource4
            // 
            this.sqlDataSource4.ConnectionString = "Data Source=.;Initial Catalog=10001;Integrated Security=True";
            this.sqlDataSource4.Name = "sqlDataSource4";
            this.sqlDataSource4.ProviderName = "System.Data.SqlClient";
            this.sqlDataSource4.SelectCommand = "SELECT        CategoryID, CategoryName\r\nFROM            Parameter";
            // 
            // M1001
            // 
            group1.GroupFooter = this.groupFooterSection;
            group1.GroupHeader = this.groupHeaderSection;
            group1.Groupings.Add(new Telerik.Reporting.Grouping("= Fields.DateOfRegistration"));
            group1.Name = "group";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.groupHeaderSection,
            this.groupFooterSection,
            this.detailSection1,
            this.reportHeaderSection1});
            this.Name = "M100BondBank";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.2D), Telerik.Reporting.Drawing.Unit.Inch(0.2D), Telerik.Reporting.Drawing.Unit.Inch(0.2D), Telerik.Reporting.Drawing.Unit.Inch(0.4D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.AutoRefresh = true;
            reportParameter1.Name = "userID";
            reportParameter2.Name = "language";
            reportParameter3.AutoRefresh = true;
            reportParameter3.Name = "pictorial";
            reportParameter3.Text = "Resimli";
            reportParameter3.Type = Telerik.Reporting.ReportParameterType.Boolean;
            reportParameter3.Value = "True\r\n";
            reportParameter4.AutoRefresh = true;
            reportParameter4.Name = "schoolCode";
            reportParameter4.Value = "invalidDatabaseName";
            reportParameter5.AutoRefresh = true;
            reportParameter5.Name = "studentID";
            reportParameter5.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter5.Value = "= Parameters.studentID.Value";
            reportParameter6.Name = "wwwRootPath";
            reportParameter6.Value = "= Parameters.wwwRootPath.Value";
            reportParameter7.AutoRefresh = true;
            reportParameter7.Name = "isFormPrint";
            reportParameter7.Text = "Form Dökümü";
            reportParameter7.Type = Telerik.Reporting.ReportParameterType.Boolean;
            reportParameter7.Value = "invalidDatabaseName";
            reportParameter8.AutoRefresh = true;
            reportParameter8.Name = "isMailOrder";
            reportParameter8.Text = "Mail Order Dökümü";
            reportParameter8.Type = Telerik.Reporting.ReportParameterType.Boolean;
            reportParameter8.Value = "True";
            reportParameter9.AutoRefresh = true;
            reportParameter9.Name = "isReceipt1";
            reportParameter9.Text = "Tahsil Makbuzu";
            reportParameter9.Type = Telerik.Reporting.ReportParameterType.Boolean;
            reportParameter9.Value = "True";
            reportParameter10.AutoRefresh = true;
            reportParameter10.Name = "isReceipt2";
            reportParameter10.Text = "Tahsil Dekontu";
            reportParameter10.Type = Telerik.Reporting.ReportParameterType.Boolean;
            reportParameter10.Value = "True";
            reportParameter11.AutoRefresh = true;
            reportParameter11.Name = "period";
            reportParameter12.AutoRefresh = true;
            reportParameter12.Name = "schoolID";
            reportParameter12.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter13.Name = "receiptNo";
            reportParameter14.Name = "formNameTitle";
            reportParameter15.Name = "formNameHeader";
            reportParameter16.Name = "formTitle";
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.ReportParameters.Add(reportParameter3);
            this.ReportParameters.Add(reportParameter4);
            this.ReportParameters.Add(reportParameter5);
            this.ReportParameters.Add(reportParameter6);
            this.ReportParameters.Add(reportParameter7);
            this.ReportParameters.Add(reportParameter8);
            this.ReportParameters.Add(reportParameter9);
            this.ReportParameters.Add(reportParameter10);
            this.ReportParameters.Add(reportParameter11);
            this.ReportParameters.Add(reportParameter12);
            this.ReportParameters.Add(reportParameter13);
            this.ReportParameters.Add(reportParameter14);
            this.ReportParameters.Add(reportParameter15);
            this.ReportParameters.Add(reportParameter16);
            renderingSettings1.Description = null;
            renderingSettings1.Name = null;
            renderingSettings1.Parameters.Add(new Telerik.Reporting.Parameter("pictorial", "= Parameters.isFormPrint.Value"));
            this.RuntimeSettings.Add(renderingSettings1);
            this.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(1D);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.Table), "Normal.TableNormal")});
            styleRule2.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule2.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule2.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            styleRule2.Style.Color = System.Drawing.Color.Black;
            styleRule2.Style.Font.Name = "Tahoma";
            styleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            descendantSelector1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.Table)),
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.ReportItem), "Normal.TableBody")});
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            descendantSelector1});
            styleRule3.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule3.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            styleRule3.Style.Font.Name = "Tahoma";
            styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            descendantSelector2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.Table)),
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.ReportItem), "Normal.TableHeader")});
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            descendantSelector2});
            styleRule4.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule4.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            styleRule4.Style.Font.Name = "Tahoma";
            styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(7.638D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.DetailSection detailSection1;
        private Telerik.Reporting.GroupHeaderSection groupHeaderSection;
        private Telerik.Reporting.GroupFooterSection groupFooterSection;
        private Telerik.Reporting.ReportHeaderSection reportHeaderSection1;
        private Telerik.Reporting.SqlDataSource sqlDataSource2;
        private Telerik.Reporting.SqlDataSource sqlDataSource1;
        private Telerik.Reporting.SqlDataSource sqlDataSource4;
        private Telerik.Reporting.SubReport TahsilMakbuzu1;
        private Telerik.Reporting.SubReport TahsilMakbuzu2;
    }
}