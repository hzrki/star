using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using WindowsApplication2.My;

namespace msphack;

[DesignerGenerated]
public class Form1 : Form
{
	private static List<WeakReference> __ENCList = new List<WeakReference>();

	private IContainer components;

	[AccessedThroughProperty("Timer1")]
	private Timer _Timer1;

	[AccessedThroughProperty("PictureBox1")]
	private PictureBox _PictureBox1;

	[AccessedThroughProperty("GroupBox1")]
	private GroupBox _GroupBox1;

	[AccessedThroughProperty("GroupBox2")]
	private GroupBox _GroupBox2;

	[AccessedThroughProperty("GroupBox3")]
	private GroupBox _GroupBox3;

	[AccessedThroughProperty("Timer2")]
	private Timer _Timer2;

	[AccessedThroughProperty("User")]
	private TextBox _User;

	[AccessedThroughProperty("Password")]
	private TextBox _Password;

	[AccessedThroughProperty("Login")]
	private Button _Login;

	[AccessedThroughProperty("Status")]
	private Label _Status;

	[AccessedThroughProperty("Label1")]
	private Label _Label1;

	[AccessedThroughProperty("Label3")]
	private Label _Label3;

	[AccessedThroughProperty("Label2")]
	private Label _Label2;

	[AccessedThroughProperty("Amount")]
	private TextBox _Amount;

	[AccessedThroughProperty("Label6")]
	private Label _Label6;

	[AccessedThroughProperty("Label5")]
	private Label _Label5;

	[AccessedThroughProperty("Label4")]
	private Label _Label4;

	[AccessedThroughProperty("Amountps")]
	private TextBox _Amountps;

	[AccessedThroughProperty("Button1")]
	private Button _Button1;

	[AccessedThroughProperty("Button2")]
	private Button _Button2;

	[AccessedThroughProperty("ProgressBar1")]
	private ProgressBar _ProgressBar1;

	[AccessedThroughProperty("how")]
	private Label _how;

	[AccessedThroughProperty("se")]
	private ComboBox _se;

	public string ahacked;

	public string art;

	public string ticket;

	private string zahl;

	public int actorid;

	private string sizee;

	private bool ausgefahren;

	private string zs;

	private int summe;

	private string en;

	internal virtual Timer Timer1
	{
		[DebuggerNonUserCode]
		get
		{
			return _Timer1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			EventHandler value2 = Timer1_Tick;
			if (_Timer1 != null)
			{
				_Timer1.Tick -= value2;
			}
			_Timer1 = value;
			if (_Timer1 != null)
			{
				_Timer1.Tick += value2;
			}
		}
	}

	internal virtual PictureBox PictureBox1
	{
		[DebuggerNonUserCode]
		get
		{
			return _PictureBox1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			EventHandler value2 = PictureBox1_Click;
			if (_PictureBox1 != null)
			{
				_PictureBox1.Click -= value2;
			}
			_PictureBox1 = value;
			if (_PictureBox1 != null)
			{
				_PictureBox1.Click += value2;
			}
		}
	}

	internal virtual GroupBox GroupBox1
	{
		[DebuggerNonUserCode]
		get
		{
			return _GroupBox1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			EventHandler value2 = GroupBox1_Enter;
			if (_GroupBox1 != null)
			{
				_GroupBox1.Enter -= value2;
			}
			_GroupBox1 = value;
			if (_GroupBox1 != null)
			{
				_GroupBox1.Enter += value2;
			}
		}
	}

	internal virtual GroupBox GroupBox2
	{
		[DebuggerNonUserCode]
		get
		{
			return _GroupBox2;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			_GroupBox2 = value;
		}
	}

	internal virtual GroupBox GroupBox3
	{
		[DebuggerNonUserCode]
		get
		{
			return _GroupBox3;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			_GroupBox3 = value;
		}
	}

	internal virtual Timer Timer2
	{
		[DebuggerNonUserCode]
		get
		{
			return _Timer2;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			EventHandler value2 = Timer2_Tick_1;
			if (_Timer2 != null)
			{
				_Timer2.Tick -= value2;
			}
			_Timer2 = value;
			if (_Timer2 != null)
			{
				_Timer2.Tick += value2;
			}
		}
	}

	internal virtual TextBox User
	{
		[DebuggerNonUserCode]
		get
		{
			return _User;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			_User = value;
		}
	}

	internal virtual TextBox Password
	{
		[DebuggerNonUserCode]
		get
		{
			return _Password;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			_Password = value;
		}
	}

	internal virtual Button Login
	{
		[DebuggerNonUserCode]
		get
		{
			return _Login;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			EventHandler value2 = Login_Click;
			if (_Login != null)
			{
				_Login.Click -= value2;
			}
			_Login = value;
			if (_Login != null)
			{
				_Login.Click += value2;
			}
		}
	}

	internal virtual Label Status
	{
		[DebuggerNonUserCode]
		get
		{
			return _Status;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			_Status = value;
		}
	}

	internal virtual Label Label1
	{
		[DebuggerNonUserCode]
		get
		{
			return _Label1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			_Label1 = value;
		}
	}

	internal virtual Label Label3
	{
		[DebuggerNonUserCode]
		get
		{
			return _Label3;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			_Label3 = value;
		}
	}

	internal virtual Label Label2
	{
		[DebuggerNonUserCode]
		get
		{
			return _Label2;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			_Label2 = value;
		}
	}

	internal virtual TextBox Amount
	{
		[DebuggerNonUserCode]
		get
		{
			return _Amount;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			_Amount = value;
		}
	}

	internal virtual Label Label6
	{
		[DebuggerNonUserCode]
		get
		{
			return _Label6;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			_Label6 = value;
		}
	}

	internal virtual Label Label5
	{
		[DebuggerNonUserCode]
		get
		{
			return _Label5;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			_Label5 = value;
		}
	}

	internal virtual Label Label4
	{
		[DebuggerNonUserCode]
		get
		{
			return _Label4;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			_Label4 = value;
		}
	}

	internal virtual TextBox Amountps
	{
		[DebuggerNonUserCode]
		get
		{
			return _Amountps;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			_Amountps = value;
		}
	}

	internal virtual Button Button1
	{
		[DebuggerNonUserCode]
		get
		{
			return _Button1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			EventHandler value2 = Button1_Click_1;
			if (_Button1 != null)
			{
				_Button1.Click -= value2;
			}
			_Button1 = value;
			if (_Button1 != null)
			{
				_Button1.Click += value2;
			}
		}
	}

	internal virtual Button Button2
	{
		[DebuggerNonUserCode]
		get
		{
			return _Button2;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			EventHandler value2 = Button2_Click_4;
			if (_Button2 != null)
			{
				_Button2.Click -= value2;
			}
			_Button2 = value;
			if (_Button2 != null)
			{
				_Button2.Click += value2;
			}
		}
	}

	internal virtual ProgressBar ProgressBar1
	{
		[DebuggerNonUserCode]
		get
		{
			return _ProgressBar1;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			_ProgressBar1 = value;
		}
	}

	internal virtual Label how
	{
		[DebuggerNonUserCode]
		get
		{
			return _how;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			_how = value;
		}
	}

	internal virtual ComboBox se
	{
		[DebuggerNonUserCode]
		get
		{
			return _se;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		[DebuggerNonUserCode]
		set
		{
			EventHandler value2 = se_SelectedIndexChanged;
			if (_se != null)
			{
				_se.SelectedIndexChanged -= value2;
			}
			_se = value;
			if (_se != null)
			{
				_se.SelectedIndexChanged += value2;
			}
		}
	}

	public Form1()
	{
		base.Load += Form1_Load;
		__ENCAddToList(this);
		ahacked = Conversions.ToString(0);
		sizee = Conversions.ToString(433);
		ausgefahren = false;
		summe = 0;
		en = ".de";
		InitializeComponent();
	}

	[DebuggerNonUserCode]
	private static void __ENCAddToList(object value)
	{
		checked
		{
			lock (__ENCList)
			{
				if (__ENCList.Count == __ENCList.Capacity)
				{
					int num = 0;
					int num2 = __ENCList.Count - 1;
					int num3 = 0;
					while (true)
					{
						int num4 = num3;
						int num5 = num2;
						if (num4 > num5)
						{
							break;
						}
						WeakReference weakReference = __ENCList[num3];
						if (weakReference.IsAlive)
						{
							if (num3 != num)
							{
								__ENCList[num] = __ENCList[num3];
							}
							num++;
						}
						num3++;
					}
					__ENCList.RemoveRange(num, __ENCList.Count - num);
					__ENCList.Capacity = __ENCList.Count;
				}
				__ENCList.Add(new WeakReference(RuntimeHelpers.GetObjectValue(value)));
			}
		}
	}

	[DebuggerNonUserCode]
	protected override void Dispose(bool disposing)
	{
		try
		{
			if ((disposing && components != null) ? true : false)
			{
				components.Dispose();
			}
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	[System.Diagnostics.DebuggerStepThrough]
	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(msphack.Form1));
		this.Timer1 = new System.Windows.Forms.Timer(this.components);
		this.PictureBox1 = new System.Windows.Forms.PictureBox();
		this.GroupBox1 = new System.Windows.Forms.GroupBox();
		this.Label3 = new System.Windows.Forms.Label();
		this.Label2 = new System.Windows.Forms.Label();
		this.Status = new System.Windows.Forms.Label();
		this.Label1 = new System.Windows.Forms.Label();
		this.Login = new System.Windows.Forms.Button();
		this.Password = new System.Windows.Forms.TextBox();
		this.User = new System.Windows.Forms.TextBox();
		this.GroupBox2 = new System.Windows.Forms.GroupBox();
		this.Button1 = new System.Windows.Forms.Button();
		this.Amountps = new System.Windows.Forms.TextBox();
		this.Amount = new System.Windows.Forms.TextBox();
		this.Label6 = new System.Windows.Forms.Label();
		this.Label5 = new System.Windows.Forms.Label();
		this.Label4 = new System.Windows.Forms.Label();
		this.GroupBox3 = new System.Windows.Forms.GroupBox();
		this.Timer2 = new System.Windows.Forms.Timer(this.components);
		this.Button2 = new System.Windows.Forms.Button();
		this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
		this.how = new System.Windows.Forms.Label();
		this.se = new System.Windows.Forms.ComboBox();
		((System.ComponentModel.ISupportInitialize)this.PictureBox1).BeginInit();
		this.GroupBox1.SuspendLayout();
		this.GroupBox2.SuspendLayout();
		this.GroupBox3.SuspendLayout();
		base.SuspendLayout();
		this.Timer1.Interval = 25;
		this.PictureBox1.Image = (System.Drawing.Image)resources.GetObject("PictureBox1.Image");
		System.Windows.Forms.PictureBox pictureBox = this.PictureBox1;
		System.Drawing.Point location = new System.Drawing.Point(51, 7);
		pictureBox.Location = location;
		this.PictureBox1.Name = "PictureBox1";
		System.Windows.Forms.PictureBox pictureBox2 = this.PictureBox1;
		System.Drawing.Size size = new System.Drawing.Size(289, 206);
		pictureBox2.Size = size;
		this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.PictureBox1.TabIndex = 15;
		this.PictureBox1.TabStop = false;
		this.GroupBox1.Controls.Add(this.se);
		this.GroupBox1.Controls.Add(this.Label3);
		this.GroupBox1.Controls.Add(this.Label2);
		this.GroupBox1.Controls.Add(this.Status);
		this.GroupBox1.Controls.Add(this.Label1);
		this.GroupBox1.Controls.Add(this.Login);
		this.GroupBox1.Controls.Add(this.Password);
		this.GroupBox1.Controls.Add(this.User);
		System.Windows.Forms.GroupBox groupBox = this.GroupBox1;
		location = new System.Drawing.Point(6, 214);
		groupBox.Location = location;
		this.GroupBox1.Name = "GroupBox1";
		System.Windows.Forms.GroupBox groupBox2 = this.GroupBox1;
		size = new System.Drawing.Size(397, 90);
		groupBox2.Size = size;
		this.GroupBox1.TabIndex = 48;
		this.GroupBox1.TabStop = false;
		this.GroupBox1.Text = "Login:";
		this.Label3.AutoSize = true;
		this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		System.Windows.Forms.Label label = this.Label3;
		location = new System.Drawing.Point(6, 41);
		label.Location = location;
		this.Label3.Name = "Label3";
		System.Windows.Forms.Label label2 = this.Label3;
		size = new System.Drawing.Size(64, 15);
		label2.Size = size;
		this.Label3.TabIndex = 62;
		this.Label3.Text = "Password:";
		this.Label2.AutoSize = true;
		this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		System.Windows.Forms.Label label3 = this.Label2;
		location = new System.Drawing.Point(6, 15);
		label3.Location = location;
		this.Label2.Name = "Label2";
		System.Windows.Forms.Label label4 = this.Label2;
		size = new System.Drawing.Size(68, 15);
		label4.Size = size;
		this.Label2.TabIndex = 61;
		this.Label2.Text = "Username:";
		this.Status.AutoSize = true;
		this.Status.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Status.ForeColor = System.Drawing.Color.Red;
		System.Windows.Forms.Label status = this.Status;
		location = new System.Drawing.Point(286, 67);
		status.Location = location;
		this.Status.Name = "Status";
		System.Windows.Forms.Label status2 = this.Status;
		size = new System.Drawing.Size(95, 15);
		status2.Size = size;
		this.Status.TabIndex = 11;
		this.Status.Text = "not logged in!";
		this.Label1.AutoSize = true;
		this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		System.Windows.Forms.Label label5 = this.Label1;
		location = new System.Drawing.Point(239, 67);
		label5.Location = location;
		this.Label1.Name = "Label1";
		System.Windows.Forms.Label label6 = this.Label1;
		size = new System.Drawing.Size(51, 15);
		label6.Size = size;
		this.Label1.TabIndex = 10;
		this.Label1.Text = "Status:";
		this.Login.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		System.Windows.Forms.Button login = this.Login;
		location = new System.Drawing.Point(9, 64);
		login.Location = location;
		this.Login.Name = "Login";
		System.Windows.Forms.Button login2 = this.Login;
		size = new System.Drawing.Size(100, 23);
		login2.Size = size;
		this.Login.TabIndex = 9;
		this.Login.Text = "Login!";
		this.Login.UseVisualStyleBackColor = true;
		System.Windows.Forms.TextBox password = this.Password;
		location = new System.Drawing.Point(88, 40);
		password.Location = location;
		this.Password.Name = "Password";
		System.Windows.Forms.TextBox password2 = this.Password;
		size = new System.Drawing.Size(293, 20);
		password2.Size = size;
		this.Password.TabIndex = 8;
		this.Password.UseSystemPasswordChar = true;
		System.Windows.Forms.TextBox user = this.User;
		location = new System.Drawing.Point(88, 14);
		user.Location = location;
		this.User.Name = "User";
		System.Windows.Forms.TextBox user2 = this.User;
		size = new System.Drawing.Size(293, 20);
		user2.Size = size;
		this.User.TabIndex = 7;
		this.GroupBox2.Controls.Add(this.Button1);
		this.GroupBox2.Controls.Add(this.Amountps);
		this.GroupBox2.Controls.Add(this.Amount);
		this.GroupBox2.Controls.Add(this.Label6);
		this.GroupBox2.Controls.Add(this.Label5);
		this.GroupBox2.Controls.Add(this.Label4);
		this.GroupBox2.Enabled = false;
		System.Windows.Forms.GroupBox groupBox3 = this.GroupBox2;
		location = new System.Drawing.Point(6, 307);
		groupBox3.Location = location;
		this.GroupBox2.Name = "GroupBox2";
		System.Windows.Forms.GroupBox groupBox4 = this.GroupBox2;
		size = new System.Drawing.Size(397, 125);
		groupBox4.Size = size;
		this.GroupBox2.TabIndex = 49;
		this.GroupBox2.TabStop = false;
		this.GroupBox2.Text = "Hack-Settings:";
		System.Windows.Forms.Button button = this.Button1;
		location = new System.Drawing.Point(138, 9);
		button.Location = location;
		this.Button1.Name = "Button1";
		System.Windows.Forms.Button button2 = this.Button1;
		size = new System.Drawing.Size(243, 23);
		button2.Size = size;
		this.Button1.TabIndex = 45;
		this.Button1.Text = "Set Account level to 6";
		this.Button1.UseVisualStyleBackColor = true;
		System.Windows.Forms.TextBox amountps = this.Amountps;
		location = new System.Drawing.Point(138, 91);
		amountps.Location = location;
		this.Amountps.Name = "Amountps";
		System.Windows.Forms.TextBox amountps2 = this.Amountps;
		size = new System.Drawing.Size(243, 20);
		amountps2.Size = size;
		this.Amountps.TabIndex = 44;
		System.Windows.Forms.TextBox amount = this.Amount;
		location = new System.Drawing.Point(138, 47);
		amount.Location = location;
		this.Amount.Name = "Amount";
		System.Windows.Forms.TextBox amount2 = this.Amount;
		size = new System.Drawing.Size(243, 20);
		amount2.Size = size;
		this.Amount.TabIndex = 43;
		this.Label6.AutoSize = true;
		this.Label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		System.Windows.Forms.Label label7 = this.Label6;
		location = new System.Drawing.Point(6, 92);
		label7.Location = location;
		this.Label6.Name = "Label6";
		System.Windows.Forms.Label label8 = this.Label6;
		size = new System.Drawing.Size(116, 15);
		label8.Size = size;
		this.Label6.TabIndex = 42;
		this.Label6.Text = "Amount per second:";
		this.Label5.AutoSize = true;
		this.Label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		System.Windows.Forms.Label label9 = this.Label5;
		location = new System.Drawing.Point(6, 48);
		label9.Location = location;
		this.Label5.Name = "Label5";
		System.Windows.Forms.Label label10 = this.Label5;
		size = new System.Drawing.Size(52, 15);
		label10.Size = size;
		this.Label5.TabIndex = 41;
		this.Label5.Text = "Amount:";
		this.Label4.AutoSize = true;
		this.Label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		System.Windows.Forms.Label label11 = this.Label4;
		location = new System.Drawing.Point(6, 12);
		label11.Location = location;
		this.Label4.Name = "Label4";
		System.Windows.Forms.Label label12 = this.Label4;
		size = new System.Drawing.Size(38, 15);
		label12.Size = size;
		this.Label4.TabIndex = 40;
		this.Label4.Text = "Extra:";
		this.GroupBox3.Controls.Add(this.how);
		this.GroupBox3.Controls.Add(this.ProgressBar1);
		this.GroupBox3.Controls.Add(this.Button2);
		this.GroupBox3.Enabled = false;
		System.Windows.Forms.GroupBox groupBox5 = this.GroupBox3;
		location = new System.Drawing.Point(6, 433);
		groupBox5.Location = location;
		this.GroupBox3.Name = "GroupBox3";
		System.Windows.Forms.GroupBox groupBox6 = this.GroupBox3;
		size = new System.Drawing.Size(397, 123);
		groupBox6.Size = size;
		this.GroupBox3.TabIndex = 50;
		this.GroupBox3.TabStop = false;
		this.GroupBox3.Text = "Hack";
		this.Timer2.Interval = 60000;
		System.Windows.Forms.Button button3 = this.Button2;
		location = new System.Drawing.Point(9, 16);
		button3.Location = location;
		this.Button2.Name = "Button2";
		System.Windows.Forms.Button button4 = this.Button2;
		size = new System.Drawing.Size(382, 45);
		button4.Size = size;
		this.Button2.TabIndex = 22;
		this.Button2.Text = "Hack!";
		this.Button2.UseVisualStyleBackColor = true;
		System.Windows.Forms.ProgressBar progressBar = this.ProgressBar1;
		location = new System.Drawing.Point(9, 67);
		progressBar.Location = location;
		this.ProgressBar1.Name = "ProgressBar1";
		System.Windows.Forms.ProgressBar progressBar2 = this.ProgressBar1;
		size = new System.Drawing.Size(382, 34);
		progressBar2.Size = size;
		this.ProgressBar1.TabIndex = 30;
		this.how.AutoSize = true;
		this.how.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		System.Windows.Forms.Label label13 = this.how;
		location = new System.Drawing.Point(151, 105);
		label13.Location = location;
		this.how.Name = "how";
		System.Windows.Forms.Label label14 = this.how;
		size = new System.Drawing.Size(82, 17);
		label14.Size = size;
		this.how.TabIndex = 31;
		this.how.Text = "0/0 hacked.";
		this.se.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.se.FormattingEnabled = true;
		this.se.Items.AddRange(new object[16]
		{
			"GB", "US", "TR", "SE", "FR", "DE", "NL", "FI", "NO", "DK",
			"CA", "AU", "PL", "NZ", "IE", "ES"
		});
		System.Windows.Forms.ComboBox comboBox = this.se;
		location = new System.Drawing.Point(115, 65);
		comboBox.Location = location;
		this.se.Name = "se";
		System.Windows.Forms.ComboBox comboBox2 = this.se;
		size = new System.Drawing.Size(118, 21);
		comboBox2.Size = size;
		this.se.TabIndex = 63;
		System.Drawing.SizeF sizeF = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleDimensions = sizeF;
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		size = new System.Drawing.Size(407, 558);
		base.ClientSize = size;
		base.Controls.Add(this.GroupBox3);
		base.Controls.Add(this.GroupBox2);
		base.Controls.Add(this.GroupBox1);
		base.Controls.Add(this.PictureBox1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "Form1";
		this.Text = "MSP Hack Remastered";
		((System.ComponentModel.ISupportInitialize)this.PictureBox1).EndInit();
		this.GroupBox1.ResumeLayout(false);
		this.GroupBox1.PerformLayout();
		this.GroupBox2.ResumeLayout(false);
		this.GroupBox2.PerformLayout();
		this.GroupBox3.ResumeLayout(false);
		this.GroupBox3.PerformLayout();
		base.ResumeLayout(false);
	}

	private void Form1_Load(object sender, EventArgs e)
	{
	}

	private void TextBox2_TextChanged(object sender, EventArgs e)
	{
	}

	public void Hack()
{
    int num = Conversions.ToInteger(Amountps.Text);
    zahl = Conversions.ToString(Conversions.ToDouble(zahl) + (double)num);
    string left = "kuchen";
    int lErl = 0;

    for (int i = 0; i < 100; i++)
    {
        if (Operators.CompareString(left, "kuchen", TextCompare: false) == 0)
        {
            try
            {
                object objMoney = AMF.AMFConn(en, "MovieStarPlanet.WebService.AMFAwardService.claimDailyAward", new object[4]
                {
                    new TicketHeader
                    {
                        anyAttribute = null,
                        Ticket = Checksum.actor(ticket)
                    },
                    "twoPlayerMoney",
                    50,
                    actorid
                });

                if (objMoney is Dictionary<string, object> dictionaryMoney && dictionaryMoney.ContainsKey("money") && dictionaryMoney["money"] is Dictionary<string, object> dictionary2Money && dictionary2Money.ContainsKey("amount") && dictionary2Money["amount"].ToString() == "50")
                {
                    ProgressBar progressBarMoney = ProgressBar1;
                    progressBarMoney.Value = checked((int)Math.Round((double)progressBarMoney.Value + Conversions.ToDouble(Amountps.Text)));
                }
                else
                {
                    lErl = 1;
                    string text = Conversions.ToString(0);
                    text = zahl;
                    zahl = Conversions.ToString(Conversions.ToDouble(text) - Conversions.ToDouble(Amountps.Text));
                    Timer2.Start();
                    Timer1.Stop();
                }

                object objFame = AMF.AMFConn(en, "MovieStarPlanet.WebService.AMFAwardService.claimDailyAward", new object[4]
                {
                    new TicketHeader
                    {
                        anyAttribute = null,
                        Ticket = Checksum.actor(ticket)
                    },
                    "twoPlayerFame",
                    50,
                    actorid
                });

                if (objFame is Dictionary<string, object> dictionaryFame && dictionaryFame.ContainsKey("fame") && dictionaryFame["fame"] is Dictionary<string, object> dictionary2Fame && dictionary2Fame.ContainsKey("amount") && dictionary2Fame["amount"].ToString() == "50")
                {
                    ProgressBar progressBarFame = ProgressBar1;
                    progressBarFame.Value = checked((int)Math.Round((double)progressBarFame.Value + Conversions.ToDouble(Amountps.Text)));
                }
                else
                {
                    lErl = 1;
                    string text = Conversions.ToString(0);
                    text = zahl;
                    zahl = Conversions.ToString(Conversions.ToDouble(text) - Conversions.ToDouble(Amountps.Text));
                    Timer2.Start();
                    Timer1.Stop();
                }
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex, lErl);
                Exception ex2 = ex;
                Timer1.Stop();
                Timer2.Start();
                ProjectData.ClearProjectError();
                return;
            }
        }
    }

    MessageBox.Show("Hacked");
}


	private void TextBox4_TextChanged(object sender, EventArgs e)
	{
	}

	private void Timer1_Tick(object sender, EventArgs e)
	{
		try
		{
			Hack();
			string text = Conversions.ToString(ProgressBar1.Value);
			how.Text = text + "/" + Amount.Text + " hacked.";
			if ((double)ProgressBar1.Value == Conversions.ToDouble(Amount.Text))
			{
				Timer1.Stop();
				ProgressBar1.Value = 0;
				zahl = Conversions.ToString(0);
				Interaction.MsgBox("Hacked", MsgBoxStyle.Information, "Success");
				summe = 2;
				ahacked = Conversions.ToString(0);
				how.Text = ahacked + "/" + Amount.Text + " hacked.";
				GroupBox1.Enabled = true;
				GroupBox3.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			Timer1.Stop();
			Timer2.Start();
			ProjectData.ClearProjectError();
		}
	}

	private void CheckBox1_CheckedChanged(object sender, EventArgs e)
	{
	}

	private void Button2_Click(object sender, EventArgs e)
	{
	}

	private void Button2_Click_1(object sender, EventArgs e)
	{
	}

	private void Timer2_Tick(object sender, EventArgs e)
	{
	}

	private void Button1_Click(object sender, EventArgs e)
	{
	}

	private void GroupBox1_Enter(object sender, EventArgs e)
	{
	}

	private void Timer2_Tick_1(object sender, EventArgs e)
	{
		Timer1.Start();
		Timer2.Stop();
	}

	private void MetroExpander1_Paint(object sender, PaintEventArgs e)
	{
	}

	private void MetroTextBox4_Click(object sender, EventArgs e)
	{
	}

	private void Button2_Click_2(object sender, EventArgs e)
	{
	}

	private void Button2_Click_3(object sender, EventArgs e)
	{
		Interaction.MsgBox("Sicherheitslücke von msp, gecoded von jack30t (skype:jack.msp) design von gather\r\nund ja, mehr gibts nich zu sagen xD", MsgBoxStyle.Information, "Über mich");
	}

	private void Button3_Click(object sender, EventArgs e)
	{
	}

	private void PictureBox1_Click(object sender, EventArgs e)
	{
	}

	private void Login_Click(object sender, EventArgs e)
	{
		dynamic val = null;
		val = AMF.AMFConn(en, "MovieStarPlanet.WebService.User.AMFUserServiceWeb.Login", new object[6]
		{
			User.Text,
			Password.Text,
			new object[1] { 134744072 },
			null,
			null,
			"MSP1-Standalone:XXXXXX"
		});
		if (val["loginStatus"]["status"] != "Success")
		{
			Status.Text = "not logged in!";
			Status.ForeColor = Color.Red;
			GroupBox2.Enabled = false;
			GroupBox3.Enabled = false;
			Timer1.Stop();
			Timer2.Stop();
			ProgressBar1.Value = 0;
			User.Text = "";
			Password.Text = "";
			Interaction.MsgBox("Password wrong or account locked!", MsgBoxStyle.Critical, "Error!");
		}
		else
		{
			Interaction.MsgBox("Logged in!", MsgBoxStyle.Information, "Logged in!");
			Status.ForeColor = Color.Green;
			Status.Text = "logged in!";
			GroupBox2.Enabled = true;
			GroupBox3.Enabled = true;
			User.Text = "";
			Password.Text = "";
			ticket = val["loginStatus"]["ticket"];
			actorid = val["loginStatus"]["actor"]["ActorId"];
		}
	}

	private void Button1_Click_1(object sender, EventArgs e)
	{
		GroupBox3.Enabled = false;
		Amount.Text = "5000";
		Amountps.Text = "1000";
		summe = 100000000;
		art = "FAME";
		ProgressBar1.Value = 0;
		string text = Amountps.Text;
		Timer1.Start();
		ProgressBar1.Maximum = Conversions.ToInteger(Amount.Text);
		ProgressBar1.Maximum = Conversions.ToInteger(Amount.Text);
	}

	private void Button2_Click_4(object sender, EventArgs e)
	{
		GroupBox3.Enabled = false;
		summe = 100000000;
		art = "STARCOINS";
		ProgressBar1.Value = 0;
		string text = Amountps.Text;
		Timer1.Start();
		ProgressBar1.Maximum = Conversions.ToInteger(Amount.Text);
		ProgressBar1.Maximum = Conversions.ToInteger(Amount.Text);
	}

	private void se_SelectedIndexChanged(object sender, EventArgs e)
	{
		en = Conversions.ToString(se.SelectedItem);
	}
}
