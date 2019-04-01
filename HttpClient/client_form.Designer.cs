namespace HttpClient
{
    partial class client_form
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(client_form));
            this.txtServer = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.send = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.imageBox = new System.Windows.Forms.PictureBox();
            this.copyImageBox = new System.Windows.Forms.PictureBox();
            this.start = new System.Windows.Forms.Button();
            this.stop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.copyImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(26, 26);
            this.txtServer.Margin = new System.Windows.Forms.Padding(6);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(550, 35);
            this.txtServer.TabIndex = 0;
            this.txtServer.Text = "http://127.0.0.1:8087/particle/heatmap";
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(26, 82);
            this.txtLog.Margin = new System.Windows.Forms.Padding(6);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(692, 132);
            this.txtLog.TabIndex = 3;
            // 
            // send
            // 
            this.send.Location = new System.Drawing.Point(26, 666);
            this.send.Margin = new System.Windows.Forms.Padding(6);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(696, 104);
            this.send.TabIndex = 4;
            this.send.Text = "发送";
            this.send.UseVisualStyleBackColor = true;
            this.send.Click += new System.EventHandler(this.send_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(26, 232);
            this.txtMsg.Margin = new System.Windows.Forms.Padding(6);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(692, 256);
            this.txtMsg.TabIndex = 5;
            // 
            // imageBox
            // 
            this.imageBox.Image = ((System.Drawing.Image)(resources.GetObject("imageBox.Image")));
            this.imageBox.Location = new System.Drawing.Point(734, 26);
            this.imageBox.Margin = new System.Windows.Forms.Padding(6);
            this.imageBox.Name = "imageBox";
            this.imageBox.Size = new System.Drawing.Size(1354, 956);
            this.imageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imageBox.TabIndex = 6;
            this.imageBox.TabStop = false;
            // 
            // copyImageBox
            // 
            this.copyImageBox.Location = new System.Drawing.Point(734, 994);
            this.copyImageBox.Margin = new System.Windows.Forms.Padding(6);
            this.copyImageBox.Name = "copyImageBox";
            this.copyImageBox.Size = new System.Drawing.Size(1354, 876);
            this.copyImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.copyImageBox.TabIndex = 6;
            this.copyImageBox.TabStop = false;
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(22, 520);
            this.start.Margin = new System.Windows.Forms.Padding(6);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(696, 104);
            this.start.TabIndex = 4;
            this.start.Text = "开始";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // stop
            // 
            this.stop.Location = new System.Drawing.Point(15, 810);
            this.stop.Margin = new System.Windows.Forms.Padding(6);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(696, 104);
            this.stop.TabIndex = 4;
            this.stop.Text = "停止";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Click += new System.EventHandler(this.stop_Click);
            // 
            // client_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2132, 1876);
            this.Controls.Add(this.copyImageBox);
            this.Controls.Add(this.imageBox);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.stop);
            this.Controls.Add(this.start);
            this.Controls.Add(this.send);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.txtServer);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "client_form";
            this.Text = "client_form";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.copyImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button send;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.PictureBox imageBox;
        private System.Windows.Forms.PictureBox copyImageBox;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Button stop;
    }
}

