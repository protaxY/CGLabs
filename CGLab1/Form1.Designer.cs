namespace CGLab1
{
    partial class Form1
    {
        
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.mark_scale_var = new System.Windows.Forms.NumericUpDown();
            this.mark_scale_label = new System.Windows.Forms.Label();
            this.curve_parametr_var = new System.Windows.Forms.NumericUpDown();
            this.curve_parametr_label = new System.Windows.Forms.Label();
            this.Y_offset_var = new System.Windows.Forms.NumericUpDown();
            this.scale_in_X_var = new System.Windows.Forms.NumericUpDown();
            this.angle_of_rotation_var = new System.Windows.Forms.NumericUpDown();
            this.X_offset_var = new System.Windows.Forms.NumericUpDown();
            this.scale_in_Y_var = new System.Windows.Forms.NumericUpDown();
            this.interpreter_var = new System.Windows.Forms.NumericUpDown();
            this.interpreter_label = new System.Windows.Forms.Label();
            this.angle_of_rotation_label = new System.Windows.Forms.Label();
            this.scale_in_XY_label = new System.Windows.Forms.Label();
            this.XY_offset_label = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.mark_scale_var)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.curve_parametr_var)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.Y_offset_var)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.scale_in_X_var)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.angle_of_rotation_var)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.X_offset_var)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.scale_in_Y_var)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.interpreter_var)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panel1.Controls.Add(this.mark_scale_var);
            this.panel1.Controls.Add(this.mark_scale_label);
            this.panel1.Controls.Add(this.curve_parametr_var);
            this.panel1.Controls.Add(this.curve_parametr_label);
            this.panel1.Controls.Add(this.Y_offset_var);
            this.panel1.Controls.Add(this.scale_in_X_var);
            this.panel1.Controls.Add(this.angle_of_rotation_var);
            this.panel1.Controls.Add(this.X_offset_var);
            this.panel1.Controls.Add(this.scale_in_Y_var);
            this.panel1.Controls.Add(this.interpreter_var);
            this.panel1.Controls.Add(this.interpreter_label);
            this.panel1.Controls.Add(this.angle_of_rotation_label);
            this.panel1.Controls.Add(this.scale_in_XY_label);
            this.panel1.Controls.Add(this.XY_offset_label);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(431, 186);
            this.panel1.TabIndex = 11;
            // 
            // mark_scale_var
            // 
            this.mark_scale_var.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.mark_scale_var.DecimalPlaces = 3;
            this.mark_scale_var.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F);
            this.mark_scale_var.Increment = new decimal(new int[] {1, 0, 0, 131072});
            this.mark_scale_var.Location = new System.Drawing.Point(246, 147);
            this.mark_scale_var.Maximum = new decimal(new int[] {10, 0, 0, 0});
            this.mark_scale_var.Name = "mark_scale_var";
            this.mark_scale_var.Size = new System.Drawing.Size(177, 31);
            this.mark_scale_var.TabIndex = 27;
            this.mark_scale_var.Value = new decimal(new int[] {5, 0, 0, 131072});
            // 
            // mark_scale_label
            // 
            this.mark_scale_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.mark_scale_label.Location = new System.Drawing.Point(12, 149);
            this.mark_scale_label.Name = "mark_scale_label";
            this.mark_scale_label.Size = new System.Drawing.Size(287, 28);
            this.mark_scale_label.TabIndex = 26;
            this.mark_scale_label.Text = "Ширина рисок:";
            // 
            // curve_parametr_var
            // 
            this.curve_parametr_var.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.curve_parametr_var.DecimalPlaces = 3;
            this.curve_parametr_var.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F);
            this.curve_parametr_var.Increment = new decimal(new int[] {1, 0, 0, 65536});
            this.curve_parametr_var.Location = new System.Drawing.Point(246, 119);
            this.curve_parametr_var.Maximum = new decimal(new int[] {10, 0, 0, 0});
            this.curve_parametr_var.Minimum = new decimal(new int[] {10, 0, 0, -2147483648});
            this.curve_parametr_var.Name = "curve_parametr_var";
            this.curve_parametr_var.Size = new System.Drawing.Size(177, 31);
            this.curve_parametr_var.TabIndex = 25;
            this.curve_parametr_var.Value = new decimal(new int[] {1, 0, 0, 0});
            this.curve_parametr_var.ValueChanged += new System.EventHandler(this.curve_parametr_var_ValueChanged);
            // 
            // curve_parametr_label
            // 
            this.curve_parametr_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.curve_parametr_label.Location = new System.Drawing.Point(12, 121);
            this.curve_parametr_label.Name = "curve_parametr_label";
            this.curve_parametr_label.Size = new System.Drawing.Size(287, 28);
            this.curve_parametr_label.TabIndex = 24;
            this.curve_parametr_label.Text = "Параметр кривой:";
            // 
            // Y_offset_var
            // 
            this.Y_offset_var.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Y_offset_var.DecimalPlaces = 3;
            this.Y_offset_var.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F);
            this.Y_offset_var.Increment = new decimal(new int[] {10, 0, 0, 0});
            this.Y_offset_var.Location = new System.Drawing.Point(340, 63);
            this.Y_offset_var.Maximum = new decimal(new int[] {1000, 0, 0, 0});
            this.Y_offset_var.Minimum = new decimal(new int[] {1000, 0, 0, -2147483648});
            this.Y_offset_var.Name = "Y_offset_var";
            this.Y_offset_var.Size = new System.Drawing.Size(83, 31);
            this.Y_offset_var.TabIndex = 23;
            this.Y_offset_var.ValueChanged += new System.EventHandler(this.Y_offset_var_ValueChanged);
            // 
            // scale_in_X_var
            // 
            this.scale_in_X_var.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.scale_in_X_var.DecimalPlaces = 3;
            this.scale_in_X_var.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F);
            this.scale_in_X_var.Increment = new decimal(new int[] {1, 0, 0, 65536});
            this.scale_in_X_var.Location = new System.Drawing.Point(246, 35);
            this.scale_in_X_var.Minimum = new decimal(new int[] {100, 0, 0, -2147483648});
            this.scale_in_X_var.Name = "scale_in_X_var";
            this.scale_in_X_var.Size = new System.Drawing.Size(88, 31);
            this.scale_in_X_var.TabIndex = 20;
            this.scale_in_X_var.Value = new decimal(new int[] {1, 0, 0, 0});
            this.scale_in_X_var.ValueChanged += new System.EventHandler(this.scale_in_X_var_ValueChanged);
            // 
            // angle_of_rotation_var
            // 
            this.angle_of_rotation_var.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.angle_of_rotation_var.DecimalPlaces = 3;
            this.angle_of_rotation_var.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F);
            this.angle_of_rotation_var.Location = new System.Drawing.Point(246, 91);
            this.angle_of_rotation_var.Maximum = new decimal(new int[] {360, 0, 0, 0});
            this.angle_of_rotation_var.Name = "angle_of_rotation_var";
            this.angle_of_rotation_var.Size = new System.Drawing.Size(177, 31);
            this.angle_of_rotation_var.TabIndex = 16;
            this.angle_of_rotation_var.ValueChanged += new System.EventHandler(this.angle_of_rotation_var_ValueChanged);
            // 
            // X_offset_var
            // 
            this.X_offset_var.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.X_offset_var.DecimalPlaces = 3;
            this.X_offset_var.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F);
            this.X_offset_var.Increment = new decimal(new int[] {10, 0, 0, 0});
            this.X_offset_var.Location = new System.Drawing.Point(246, 63);
            this.X_offset_var.Maximum = new decimal(new int[] {1000, 0, 0, 0});
            this.X_offset_var.Minimum = new decimal(new int[] {1000, 0, 0, -2147483648});
            this.X_offset_var.Name = "X_offset_var";
            this.X_offset_var.Size = new System.Drawing.Size(88, 31);
            this.X_offset_var.TabIndex = 15;
            this.X_offset_var.ValueChanged += new System.EventHandler(this.X_offset_var_ValueChanged);
            // 
            // scale_in_Y_var
            // 
            this.scale_in_Y_var.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.scale_in_Y_var.DecimalPlaces = 3;
            this.scale_in_Y_var.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F);
            this.scale_in_Y_var.Increment = new decimal(new int[] {1, 0, 0, 65536});
            this.scale_in_Y_var.Location = new System.Drawing.Point(340, 35);
            this.scale_in_Y_var.Minimum = new decimal(new int[] {100, 0, 0, -2147483648});
            this.scale_in_Y_var.Name = "scale_in_Y_var";
            this.scale_in_Y_var.Size = new System.Drawing.Size(83, 31);
            this.scale_in_Y_var.TabIndex = 14;
            this.scale_in_Y_var.Value = new decimal(new int[] {1, 0, 0, 0});
            this.scale_in_Y_var.ValueChanged += new System.EventHandler(this.scale_in_Y_var_ValueChanged);
            // 
            // interpreter_var
            // 
            this.interpreter_var.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.interpreter_var.DecimalPlaces = 3;
            this.interpreter_var.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F);
            this.interpreter_var.Increment = new decimal(new int[] {1, 0, 0, 65536});
            this.interpreter_var.Location = new System.Drawing.Point(246, 7);
            this.interpreter_var.Maximum = new decimal(new int[] {1, 0, 0, 0});
            this.interpreter_var.Minimum = new decimal(new int[] {1, 0, 0, 196608});
            this.interpreter_var.Name = "interpreter_var";
            this.interpreter_var.Size = new System.Drawing.Size(177, 31);
            this.interpreter_var.TabIndex = 13;
            this.interpreter_var.Value = new decimal(new int[] {1, 0, 0, 65536});
            this.interpreter_var.ValueChanged += new System.EventHandler(this.interpreter_var_ValueChanged);
            // 
            // interpreter_label
            // 
            this.interpreter_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.interpreter_label.Location = new System.Drawing.Point(12, 9);
            this.interpreter_label.Name = "interpreter_label";
            this.interpreter_label.Size = new System.Drawing.Size(287, 28);
            this.interpreter_label.TabIndex = 2;
            this.interpreter_label.Text = "Апроксимация:";
            // 
            // angle_of_rotation_label
            // 
            this.angle_of_rotation_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.angle_of_rotation_label.Location = new System.Drawing.Point(12, 93);
            this.angle_of_rotation_label.Name = "angle_of_rotation_label";
            this.angle_of_rotation_label.Size = new System.Drawing.Size(287, 28);
            this.angle_of_rotation_label.TabIndex = 6;
            this.angle_of_rotation_label.Text = "Угол поворота:";
            // 
            // scale_in_XY_label
            // 
            this.scale_in_XY_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.scale_in_XY_label.Location = new System.Drawing.Point(12, 37);
            this.scale_in_XY_label.Name = "scale_in_XY_label";
            this.scale_in_XY_label.Size = new System.Drawing.Size(287, 28);
            this.scale_in_XY_label.TabIndex = 3;
            this.scale_in_XY_label.Text = "Масштаб по X, Y:";
            // 
            // XY_offset_label
            // 
            this.XY_offset_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.XY_offset_label.Location = new System.Drawing.Point(12, 65);
            this.XY_offset_label.Name = "XY_offset_label";
            this.XY_offset_label.Size = new System.Drawing.Size(287, 28);
            this.XY_offset_label.TabIndex = 4;
            this.XY_offset_label.Text = "Смещение по X, Y:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(1259, 799);
            this.Controls.Add(this.panel1);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Location = new System.Drawing.Point(15, 15);
            this.Name = "Form1";
            this.Text = "CGLab1";
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.mark_scale_var)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.curve_parametr_var)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.Y_offset_var)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.scale_in_X_var)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.angle_of_rotation_var)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.X_offset_var)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.scale_in_Y_var)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.interpreter_var)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.NumericUpDown mark_scale_var;

        private System.Windows.Forms.Label mark_scale_label;

        private System.Windows.Forms.NumericUpDown numericUpDown2;

        private System.Windows.Forms.Label curve_parametr_label;
        
        private System.Windows.Forms.NumericUpDown curve_parametr_var;

        private System.Windows.Forms.NumericUpDown numericUpDown1;

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown Y_offset_var;
        private System.Windows.Forms.NumericUpDown scale_in_X_var;
        private System.Windows.Forms.NumericUpDown angle_of_rotation_var;
        private System.Windows.Forms.NumericUpDown X_offset_var;
        private System.Windows.Forms.NumericUpDown scale_in_Y_var;
        private System.Windows.Forms.NumericUpDown interpreter_var;
        private System.Windows.Forms.Label interpreter_label;
        private System.Windows.Forms.Label angle_of_rotation_label;
        private System.Windows.Forms.Label scale_in_XY_label;
        private System.Windows.Forms.Label XY_offset_label;

        #endregion
    }
}