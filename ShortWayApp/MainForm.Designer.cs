
namespace ShortWayApp
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.shortWayControl1 = new ShortWayApp.ShortWayControl.ShortWayControl();
            this.SuspendLayout();
            // 
            // shortWayControl1
            // 
            this.shortWayControl1.Location = new System.Drawing.Point(12, 12);
            this.shortWayControl1.Name = "shortWayControl1";
            this.shortWayControl1.Size = new System.Drawing.Size(353, 426);
            this.shortWayControl1.TabIndex = 0;
            this.shortWayControl1.ZoomCam = 1F;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.shortWayControl1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private ShortWayControl.ShortWayControl shortWayControl1;
    }
}

