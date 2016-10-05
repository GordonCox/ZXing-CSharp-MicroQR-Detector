namespace WindowsFormsDemo
{
   partial class WindowsFormsDemoForm
   {
      /// <summary>
      /// Erforderliche Designervariable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Verwendete Ressourcen bereinigen.
      /// </summary>
      /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Vom Windows Form-Designer generierter Code

      /// <summary>
      /// Erforderliche Methode für die Designerunterstützung.
      /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
      /// </summary>
      private void InitializeComponent()
      {
            this.btnClose = new System.Windows.Forms.Button();
            this.txtBarcodeImageFile = new System.Windows.Forms.TextBox();
            this.labBarcodeImageFile = new System.Windows.Forms.Label();
            this.tabCtrlMain = new System.Windows.Forms.TabControl();
            this.tabPageDecoder = new System.Windows.Forms.TabPage();
            this.btnExtendedResult = new System.Windows.Forms.Button();
            this.btnDecodingOptions = new System.Windows.Forms.Button();
            this.labDuration = new System.Windows.Forms.Label();
            this.labBarcodeText = new System.Windows.Forms.Label();
            this.labType = new System.Windows.Forms.Label();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.txtType = new System.Windows.Forms.TextBox();
            this.btnStartDecoding = new System.Windows.Forms.Button();
            this.picBarcode = new System.Windows.Forms.PictureBox();
            this.btnSelectBarcodeImageFileForDecoding = new System.Windows.Forms.Button();
            this.tabPageEncoder = new System.Windows.Forms.TabPage();
            this.btnEncodeOptions = new System.Windows.Forms.Button();
            this.btnEncodeDecode = new System.Windows.Forms.Button();
            this.btnEncoderSave = new System.Windows.Forms.Button();
            this.btnEncode = new System.Windows.Forms.Button();
            this.txtEncoderContent = new System.Windows.Forms.TextBox();
            this.labEncoderContent = new System.Windows.Forms.Label();
            this.labEncoderType = new System.Windows.Forms.Label();
            this.cmbEncoderType = new System.Windows.Forms.ComboBox();
            this.picEncodedBarCode = new System.Windows.Forms.PictureBox();
            this.tabPageWebCam = new System.Windows.Forms.TabPage();
            this.btnDecodeWebCam = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtContentWebCam = new System.Windows.Forms.TextBox();
            this.txtTypeWebCam = new System.Windows.Forms.TextBox();
            this.picWebCam = new System.Windows.Forms.PictureBox();
            this.btnResize = new System.Windows.Forms.Button();
            this.tabCtrlMain.SuspendLayout();
            this.tabPageDecoder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBarcode)).BeginInit();
            this.tabPageEncoder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEncodedBarCode)).BeginInit();
            this.tabPageWebCam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWebCam)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(474, 376);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(134, 29);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtBarcodeImageFile
            // 
            this.txtBarcodeImageFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBarcodeImageFile.Location = new System.Drawing.Point(11, 35);
            this.txtBarcodeImageFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBarcodeImageFile.Name = "txtBarcodeImageFile";
            this.txtBarcodeImageFile.Size = new System.Drawing.Size(523, 22);
            this.txtBarcodeImageFile.TabIndex = 1;
            this.txtBarcodeImageFile.TextChanged += new System.EventHandler(this.txtBarcodeImageFile_TextChanged);
            // 
            // labBarcodeImageFile
            // 
            this.labBarcodeImageFile.AutoSize = true;
            this.labBarcodeImageFile.Location = new System.Drawing.Point(8, 15);
            this.labBarcodeImageFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labBarcodeImageFile.Name = "labBarcodeImageFile";
            this.labBarcodeImageFile.Size = new System.Drawing.Size(129, 17);
            this.labBarcodeImageFile.TabIndex = 2;
            this.labBarcodeImageFile.Text = "Barcode Image File";
            // 
            // tabCtrlMain
            // 
            this.tabCtrlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCtrlMain.Controls.Add(this.tabPageDecoder);
            this.tabCtrlMain.Controls.Add(this.tabPageEncoder);
            this.tabCtrlMain.Controls.Add(this.tabPageWebCam);
            this.tabCtrlMain.Location = new System.Drawing.Point(15, 15);
            this.tabCtrlMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabCtrlMain.Name = "tabCtrlMain";
            this.tabCtrlMain.SelectedIndex = 0;
            this.tabCtrlMain.Size = new System.Drawing.Size(592, 354);
            this.tabCtrlMain.TabIndex = 3;
            // 
            // tabPageDecoder
            // 
            this.tabPageDecoder.Controls.Add(this.btnExtendedResult);
            this.tabPageDecoder.Controls.Add(this.btnDecodingOptions);
            this.tabPageDecoder.Controls.Add(this.labDuration);
            this.tabPageDecoder.Controls.Add(this.labBarcodeText);
            this.tabPageDecoder.Controls.Add(this.labType);
            this.tabPageDecoder.Controls.Add(this.txtContent);
            this.tabPageDecoder.Controls.Add(this.txtType);
            this.tabPageDecoder.Controls.Add(this.btnStartDecoding);
            this.tabPageDecoder.Controls.Add(this.picBarcode);
            this.tabPageDecoder.Controls.Add(this.btnSelectBarcodeImageFileForDecoding);
            this.tabPageDecoder.Controls.Add(this.labBarcodeImageFile);
            this.tabPageDecoder.Controls.Add(this.txtBarcodeImageFile);
            this.tabPageDecoder.Location = new System.Drawing.Point(4, 25);
            this.tabPageDecoder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageDecoder.Name = "tabPageDecoder";
            this.tabPageDecoder.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageDecoder.Size = new System.Drawing.Size(584, 325);
            this.tabPageDecoder.TabIndex = 0;
            this.tabPageDecoder.Text = "Decoder";
            this.tabPageDecoder.UseVisualStyleBackColor = true;
            // 
            // btnExtendedResult
            // 
            this.btnExtendedResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExtendedResult.Location = new System.Drawing.Point(340, 99);
            this.btnExtendedResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExtendedResult.Name = "btnExtendedResult";
            this.btnExtendedResult.Size = new System.Drawing.Size(94, 29);
            this.btnExtendedResult.TabIndex = 12;
            this.btnExtendedResult.Text = "Result";
            this.btnExtendedResult.UseVisualStyleBackColor = true;
            this.btnExtendedResult.Visible = false;
            this.btnExtendedResult.Click += new System.EventHandler(this.btnExtendedResult_Click);
            // 
            // btnDecodingOptions
            // 
            this.btnDecodingOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDecodingOptions.Location = new System.Drawing.Point(441, 99);
            this.btnDecodingOptions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDecodingOptions.Name = "btnDecodingOptions";
            this.btnDecodingOptions.Size = new System.Drawing.Size(134, 29);
            this.btnDecodingOptions.TabIndex = 11;
            this.btnDecodingOptions.Text = "Options";
            this.btnDecodingOptions.UseVisualStyleBackColor = true;
            this.btnDecodingOptions.Click += new System.EventHandler(this.btnDecodingOptions_Click);
            // 
            // labDuration
            // 
            this.labDuration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labDuration.Location = new System.Drawing.Point(286, 75);
            this.labDuration.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labDuration.Name = "labDuration";
            this.labDuration.Size = new System.Drawing.Size(148, 29);
            this.labDuration.TabIndex = 10;
            // 
            // labBarcodeText
            // 
            this.labBarcodeText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labBarcodeText.AutoSize = true;
            this.labBarcodeText.Location = new System.Drawing.Point(286, 160);
            this.labBarcodeText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labBarcodeText.Name = "labBarcodeText";
            this.labBarcodeText.Size = new System.Drawing.Size(57, 17);
            this.labBarcodeText.TabIndex = 9;
            this.labBarcodeText.Text = "Content";
            // 
            // labType
            // 
            this.labType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labType.AutoSize = true;
            this.labType.Location = new System.Drawing.Point(286, 111);
            this.labType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labType.Name = "labType";
            this.labType.Size = new System.Drawing.Size(40, 17);
            this.labType.TabIndex = 8;
            this.labType.Text = "Type";
            // 
            // txtContent
            // 
            this.txtContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContent.Location = new System.Drawing.Point(286, 180);
            this.txtContent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtContent.Size = new System.Drawing.Size(288, 133);
            this.txtContent.TabIndex = 7;
            // 
            // txtType
            // 
            this.txtType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtType.Location = new System.Drawing.Point(286, 131);
            this.txtType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(288, 22);
            this.txtType.TabIndex = 6;
            // 
            // btnStartDecoding
            // 
            this.btnStartDecoding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartDecoding.Location = new System.Drawing.Point(441, 69);
            this.btnStartDecoding.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStartDecoding.Name = "btnStartDecoding";
            this.btnStartDecoding.Size = new System.Drawing.Size(134, 29);
            this.btnStartDecoding.TabIndex = 5;
            this.btnStartDecoding.Text = "Decode";
            this.btnStartDecoding.UseVisualStyleBackColor = true;
            this.btnStartDecoding.Click += new System.EventHandler(this.btnStartDecoding_Click);
            // 
            // picBarcode
            // 
            this.picBarcode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picBarcode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picBarcode.Location = new System.Drawing.Point(11, 68);
            this.picBarcode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picBarcode.Name = "picBarcode";
            this.picBarcode.Size = new System.Drawing.Size(266, 245);
            this.picBarcode.TabIndex = 4;
            this.picBarcode.TabStop = false;
            // 
            // btnSelectBarcodeImageFileForDecoding
            // 
            this.btnSelectBarcodeImageFileForDecoding.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectBarcodeImageFileForDecoding.Location = new System.Drawing.Point(542, 32);
            this.btnSelectBarcodeImageFileForDecoding.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSelectBarcodeImageFileForDecoding.Name = "btnSelectBarcodeImageFileForDecoding";
            this.btnSelectBarcodeImageFileForDecoding.Size = new System.Drawing.Size(32, 29);
            this.btnSelectBarcodeImageFileForDecoding.TabIndex = 3;
            this.btnSelectBarcodeImageFileForDecoding.Text = "...";
            this.btnSelectBarcodeImageFileForDecoding.UseVisualStyleBackColor = true;
            this.btnSelectBarcodeImageFileForDecoding.Click += new System.EventHandler(this.btnSelectBarcodeImageFileForDecoding_Click);
            // 
            // tabPageEncoder
            // 
            this.tabPageEncoder.Controls.Add(this.btnResize);
            this.tabPageEncoder.Controls.Add(this.btnEncodeOptions);
            this.tabPageEncoder.Controls.Add(this.btnEncodeDecode);
            this.tabPageEncoder.Controls.Add(this.btnEncoderSave);
            this.tabPageEncoder.Controls.Add(this.btnEncode);
            this.tabPageEncoder.Controls.Add(this.txtEncoderContent);
            this.tabPageEncoder.Controls.Add(this.labEncoderContent);
            this.tabPageEncoder.Controls.Add(this.labEncoderType);
            this.tabPageEncoder.Controls.Add(this.cmbEncoderType);
            this.tabPageEncoder.Controls.Add(this.picEncodedBarCode);
            this.tabPageEncoder.Location = new System.Drawing.Point(4, 25);
            this.tabPageEncoder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageEncoder.Name = "tabPageEncoder";
            this.tabPageEncoder.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageEncoder.Size = new System.Drawing.Size(584, 325);
            this.tabPageEncoder.TabIndex = 1;
            this.tabPageEncoder.Text = "Encoder";
            this.tabPageEncoder.UseVisualStyleBackColor = true;
            // 
            // btnEncodeOptions
            // 
            this.btnEncodeOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncodeOptions.Location = new System.Drawing.Point(159, 280);
            this.btnEncodeOptions.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEncodeOptions.Name = "btnEncodeOptions";
            this.btnEncodeOptions.Size = new System.Drawing.Size(134, 29);
            this.btnEncodeOptions.TabIndex = 17;
            this.btnEncodeOptions.Text = "Options";
            this.btnEncodeOptions.UseVisualStyleBackColor = true;
            this.btnEncodeOptions.Click += new System.EventHandler(this.btnEncodeOptions_Click);
            // 
            // btnEncodeDecode
            // 
            this.btnEncodeDecode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEncodeDecode.Location = new System.Drawing.Point(4, 280);
            this.btnEncodeDecode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEncodeDecode.Name = "btnEncodeDecode";
            this.btnEncodeDecode.Size = new System.Drawing.Size(134, 29);
            this.btnEncodeDecode.TabIndex = 16;
            this.btnEncodeDecode.Text = "Decode";
            this.btnEncodeDecode.UseVisualStyleBackColor = true;
            this.btnEncodeDecode.Click += new System.EventHandler(this.btnEncodeDecode_Click);
            // 
            // btnEncoderSave
            // 
            this.btnEncoderSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncoderSave.Location = new System.Drawing.Point(300, 280);
            this.btnEncoderSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEncoderSave.Name = "btnEncoderSave";
            this.btnEncoderSave.Size = new System.Drawing.Size(134, 29);
            this.btnEncoderSave.TabIndex = 15;
            this.btnEncoderSave.Text = "Save";
            this.btnEncoderSave.UseVisualStyleBackColor = true;
            this.btnEncoderSave.Click += new System.EventHandler(this.btnEncoderSave_Click);
            // 
            // btnEncode
            // 
            this.btnEncode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEncode.Location = new System.Drawing.Point(441, 280);
            this.btnEncode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(134, 29);
            this.btnEncode.TabIndex = 14;
            this.btnEncode.Text = "Encode";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.btnEncode_Click);
            // 
            // txtEncoderContent
            // 
            this.txtEncoderContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEncoderContent.Location = new System.Drawing.Point(286, 79);
            this.txtEncoderContent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtEncoderContent.Multiline = true;
            this.txtEncoderContent.Name = "txtEncoderContent";
            this.txtEncoderContent.Size = new System.Drawing.Size(288, 133);
            this.txtEncoderContent.TabIndex = 13;
            // 
            // labEncoderContent
            // 
            this.labEncoderContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labEncoderContent.AutoSize = true;
            this.labEncoderContent.Location = new System.Drawing.Point(282, 59);
            this.labEncoderContent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labEncoderContent.Name = "labEncoderContent";
            this.labEncoderContent.Size = new System.Drawing.Size(57, 17);
            this.labEncoderContent.TabIndex = 12;
            this.labEncoderContent.Text = "Content";
            // 
            // labEncoderType
            // 
            this.labEncoderType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labEncoderType.AutoSize = true;
            this.labEncoderType.Location = new System.Drawing.Point(282, 9);
            this.labEncoderType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labEncoderType.Name = "labEncoderType";
            this.labEncoderType.Size = new System.Drawing.Size(40, 17);
            this.labEncoderType.TabIndex = 11;
            this.labEncoderType.Text = "Type";
            // 
            // cmbEncoderType
            // 
            this.cmbEncoderType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbEncoderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEncoderType.FormattingEnabled = true;
            this.cmbEncoderType.Location = new System.Drawing.Point(286, 29);
            this.cmbEncoderType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbEncoderType.Name = "cmbEncoderType";
            this.cmbEncoderType.Size = new System.Drawing.Size(288, 24);
            this.cmbEncoderType.TabIndex = 10;
            // 
            // picEncodedBarCode
            // 
            this.picEncodedBarCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picEncodedBarCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picEncodedBarCode.Location = new System.Drawing.Point(4, 8);
            this.picEncodedBarCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picEncodedBarCode.Name = "picEncodedBarCode";
            this.picEncodedBarCode.Size = new System.Drawing.Size(266, 264);
            this.picEncodedBarCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picEncodedBarCode.TabIndex = 9;
            this.picEncodedBarCode.TabStop = false;
            // 
            // tabPageWebCam
            // 
            this.tabPageWebCam.Controls.Add(this.btnDecodeWebCam);
            this.tabPageWebCam.Controls.Add(this.label1);
            this.tabPageWebCam.Controls.Add(this.label2);
            this.tabPageWebCam.Controls.Add(this.txtContentWebCam);
            this.tabPageWebCam.Controls.Add(this.txtTypeWebCam);
            this.tabPageWebCam.Controls.Add(this.picWebCam);
            this.tabPageWebCam.Location = new System.Drawing.Point(4, 25);
            this.tabPageWebCam.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageWebCam.Name = "tabPageWebCam";
            this.tabPageWebCam.Size = new System.Drawing.Size(584, 325);
            this.tabPageWebCam.TabIndex = 2;
            this.tabPageWebCam.Text = "WebCam";
            this.tabPageWebCam.UseVisualStyleBackColor = true;
            // 
            // btnDecodeWebCam
            // 
            this.btnDecodeWebCam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDecodeWebCam.Location = new System.Drawing.Point(434, 71);
            this.btnDecodeWebCam.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDecodeWebCam.Name = "btnDecodeWebCam";
            this.btnDecodeWebCam.Size = new System.Drawing.Size(134, 29);
            this.btnDecodeWebCam.TabIndex = 13;
            this.btnDecodeWebCam.Text = "Decode";
            this.btnDecodeWebCam.UseVisualStyleBackColor = true;
            this.btnDecodeWebCam.Click += new System.EventHandler(this.btnDecodeWebCam_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(279, 164);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 17);
            this.label1.TabIndex = 12;
            this.label1.Text = "Content";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(279, 115);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "Type";
            // 
            // txtContentWebCam
            // 
            this.txtContentWebCam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContentWebCam.Location = new System.Drawing.Point(279, 184);
            this.txtContentWebCam.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtContentWebCam.Multiline = true;
            this.txtContentWebCam.Name = "txtContentWebCam";
            this.txtContentWebCam.Size = new System.Drawing.Size(288, 133);
            this.txtContentWebCam.TabIndex = 10;
            // 
            // txtTypeWebCam
            // 
            this.txtTypeWebCam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTypeWebCam.Location = new System.Drawing.Point(279, 135);
            this.txtTypeWebCam.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTypeWebCam.Name = "txtTypeWebCam";
            this.txtTypeWebCam.Size = new System.Drawing.Size(288, 22);
            this.txtTypeWebCam.TabIndex = 9;
            // 
            // picWebCam
            // 
            this.picWebCam.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picWebCam.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picWebCam.Location = new System.Drawing.Point(4, 71);
            this.picWebCam.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picWebCam.Name = "picWebCam";
            this.picWebCam.Size = new System.Drawing.Size(266, 245);
            this.picWebCam.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picWebCam.TabIndex = 8;
            this.picWebCam.TabStop = false;
            // 
            // btnResize
            // 
            this.btnResize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResize.Location = new System.Drawing.Point(440, 243);
            this.btnResize.Margin = new System.Windows.Forms.Padding(4);
            this.btnResize.Name = "btnResize";
            this.btnResize.Size = new System.Drawing.Size(134, 29);
            this.btnResize.TabIndex = 18;
            this.btnResize.Text = "Resize";
            this.btnResize.UseVisualStyleBackColor = true;
            this.btnResize.Click += new System.EventHandler(this.button1_Click);
            // 
            // WindowsFormsDemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(622, 420);
            this.Controls.Add(this.tabCtrlMain);
            this.Controls.Add(this.btnClose);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(638, 456);
            this.Name = "WindowsFormsDemoForm";
            this.Text = "WindowsFormsDemo";
            this.tabCtrlMain.ResumeLayout(false);
            this.tabPageDecoder.ResumeLayout(false);
            this.tabPageDecoder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBarcode)).EndInit();
            this.tabPageEncoder.ResumeLayout(false);
            this.tabPageEncoder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEncodedBarCode)).EndInit();
            this.tabPageWebCam.ResumeLayout(false);
            this.tabPageWebCam.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWebCam)).EndInit();
            this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button btnClose;
      private System.Windows.Forms.TextBox txtBarcodeImageFile;
      private System.Windows.Forms.Label labBarcodeImageFile;
      private System.Windows.Forms.TabControl tabCtrlMain;
      private System.Windows.Forms.TabPage tabPageDecoder;
      private System.Windows.Forms.TabPage tabPageEncoder;
      private System.Windows.Forms.Button btnSelectBarcodeImageFileForDecoding;
      private System.Windows.Forms.Button btnStartDecoding;
      private System.Windows.Forms.PictureBox picBarcode;
      private System.Windows.Forms.Label labType;
      private System.Windows.Forms.TextBox txtContent;
      private System.Windows.Forms.TextBox txtType;
      private System.Windows.Forms.Label labBarcodeText;
      private System.Windows.Forms.Label labDuration;
      private System.Windows.Forms.TabPage tabPageWebCam;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox txtContentWebCam;
      private System.Windows.Forms.TextBox txtTypeWebCam;
      private System.Windows.Forms.PictureBox picWebCam;
      private System.Windows.Forms.Button btnDecodeWebCam;
      private System.Windows.Forms.Button btnEncode;
      private System.Windows.Forms.TextBox txtEncoderContent;
      private System.Windows.Forms.Label labEncoderContent;
      private System.Windows.Forms.Label labEncoderType;
      private System.Windows.Forms.ComboBox cmbEncoderType;
      private System.Windows.Forms.PictureBox picEncodedBarCode;
      private System.Windows.Forms.Button btnEncoderSave;
      private System.Windows.Forms.Button btnEncodeDecode;
      private System.Windows.Forms.Button btnEncodeOptions;
      private System.Windows.Forms.Button btnDecodingOptions;
      private System.Windows.Forms.Button btnExtendedResult;
        private System.Windows.Forms.Button btnResize;
    }
}

