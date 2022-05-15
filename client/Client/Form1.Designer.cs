namespace Client
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.username_textbox = new System.Windows.Forms.TextBox();
            this.ip_textbox = new System.Windows.Forms.TextBox();
            this.port_textbox = new System.Windows.Forms.TextBox();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.connect_button = new System.Windows.Forms.Button();
            this.disconnect_button = new System.Windows.Forms.Button();
            this.post_Send_Button = new System.Windows.Forms.Button();
            this.post_textbox = new System.Windows.Forms.TextBox();
            this.all_posts_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(74, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(57, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(2, 176);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 25);
            this.label5.TabIndex = 4;
            this.label5.Text = "Username:";
            // 
            // username_textbox
            // 
            this.username_textbox.Location = new System.Drawing.Point(106, 181);
            this.username_textbox.Name = "username_textbox";
            this.username_textbox.Size = new System.Drawing.Size(110, 20);
            this.username_textbox.TabIndex = 8;
            // 
            // ip_textbox
            // 
            this.ip_textbox.Location = new System.Drawing.Point(106, 67);
            this.ip_textbox.Name = "ip_textbox";
            this.ip_textbox.Size = new System.Drawing.Size(110, 20);
            this.ip_textbox.TabIndex = 10;
            // 
            // port_textbox
            // 
            this.port_textbox.Location = new System.Drawing.Point(106, 126);
            this.port_textbox.Name = "port_textbox";
            this.port_textbox.Size = new System.Drawing.Size(110, 20);
            this.port_textbox.TabIndex = 11;
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(346, 32);
            this.logs.Name = "logs";
            this.logs.Size = new System.Drawing.Size(246, 455);
            this.logs.TabIndex = 12;
            this.logs.Text = "";
            // 
            // connect_button
            // 
            this.connect_button.Location = new System.Drawing.Point(254, 61);
            this.connect_button.Name = "connect_button";
            this.connect_button.Size = new System.Drawing.Size(81, 32);
            this.connect_button.TabIndex = 13;
            this.connect_button.Text = "Connect";
            this.connect_button.UseVisualStyleBackColor = true;
            this.connect_button.Click += new System.EventHandler(this.connect_button_Click);
            // 
            // disconnect_button
            // 
            this.disconnect_button.Location = new System.Drawing.Point(254, 118);
            this.disconnect_button.Name = "disconnect_button";
            this.disconnect_button.Size = new System.Drawing.Size(81, 34);
            this.disconnect_button.TabIndex = 15;
            this.disconnect_button.Text = "Disconnect";
            this.disconnect_button.UseVisualStyleBackColor = true;
            this.disconnect_button.Click += new System.EventHandler(this.disconnect_button_Click);
            // 
            // post_Send_Button
            // 
            this.post_Send_Button.Location = new System.Drawing.Point(254, 390);
            this.post_Send_Button.Name = "post_Send_Button";
            this.post_Send_Button.Size = new System.Drawing.Size(81, 32);
            this.post_Send_Button.TabIndex = 16;
            this.post_Send_Button.Text = "Send";
            this.post_Send_Button.UseVisualStyleBackColor = true;
            this.post_Send_Button.Click += new System.EventHandler(this.post_Send_Button_Click);
            // 
            // post_textbox
            // 
            this.post_textbox.Location = new System.Drawing.Point(138, 396);
            this.post_textbox.Name = "post_textbox";
            this.post_textbox.Size = new System.Drawing.Size(110, 20);
            this.post_textbox.TabIndex = 17;
            // 
            // all_posts_button
            // 
            this.all_posts_button.Location = new System.Drawing.Point(254, 455);
            this.all_posts_button.Name = "all_posts_button";
            this.all_posts_button.Size = new System.Drawing.Size(81, 32);
            this.all_posts_button.TabIndex = 18;
            this.all_posts_button.Text = "All Posts";
            this.all_posts_button.UseVisualStyleBackColor = true;
            this.all_posts_button.Click += new System.EventHandler(this.all_posts_button_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(75, 391);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 25);
            this.label3.TabIndex = 19;
            this.label3.Text = "Post:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 581);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.all_posts_button);
            this.Controls.Add(this.post_textbox);
            this.Controls.Add(this.post_Send_Button);
            this.Controls.Add(this.disconnect_button);
            this.Controls.Add(this.connect_button);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.port_textbox);
            this.Controls.Add(this.ip_textbox);
            this.Controls.Add(this.username_textbox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox username_textbox;
        private System.Windows.Forms.TextBox ip_textbox;
        private System.Windows.Forms.TextBox port_textbox;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.Button connect_button;
        private System.Windows.Forms.Button disconnect_button;
        private System.Windows.Forms.Button post_Send_Button;
        private System.Windows.Forms.TextBox post_textbox;
        private System.Windows.Forms.Button all_posts_button;
        private System.Windows.Forms.Label label3;
    }
}

