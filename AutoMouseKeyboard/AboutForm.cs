
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AutoMouseKeyboard.Utilities;

namespace AutoMouseKeyboard
{
    public partial class AboutForm : Form
    {
    private const string BankName = "Viettinbank";
    private const string AccountNumber = "106875866606";
    private const string AccountHolder = "NGUYEN DUC QUI";

    public AboutForm()
    {
        InitializeComponent();
        ThemeManager.RegisterForm(this);
        LanguageManager.RegisterForm(this);
        LanguageManager.LanguageChanged += LanguageManager_LanguageChanged;
        Load += AboutForm_Load;
        UpdateLanguage();
    }

    private void LanguageManager_LanguageChanged(object? sender, LanguageChangedEventArgs e)
    {
        UpdateLanguage();
    }

    private void AboutForm_Load(object? sender, EventArgs e)
    {
        GenerateDonateInfo();
        LoadQRCode();
    }

    private void GenerateDonateInfo()
    {
        var donateText = string.Format(
            LanguageManager.GetString("AboutForm_DonateInfo"),
            BankName, AccountNumber, AccountHolder);
        
        lblDonateInfo.Text = donateText;
    }

    public void UpdateLanguage()
    {
        Text = LanguageManager.GetString("AboutForm_Title");
        lblTitle.Text = LanguageManager.GetString("AboutForm_AppName");
        lblVersion.Text = LanguageManager.GetString("AboutForm_Version");
        lblAuthor.Text = LanguageManager.GetString("AboutForm_Author");
        lblDescription.Text = LanguageManager.GetString("AboutForm_Description");
        lblDonateTitle.Text = LanguageManager.GetString("AboutForm_DonateTitle");
        btnOk.Text = LanguageManager.GetString("SettingForm_OK");
        GenerateDonateInfo();
    }

    private void LoadQRCode()
    {
        try
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            
            var qrStream = assembly.GetManifestResourceStream("AutoMouseKeyboard.assets.qr.jpg");
            if (qrStream != null)
            {
                picQRCode.Image = new Bitmap(qrStream);
                return;
            }

            var currentAssetsQr = Path.Combine(Directory.GetCurrentDirectory(), "assets", "qr.jpg");
            if (File.Exists(currentAssetsQr))
            {
                picQRCode.Image = new Bitmap(currentAssetsQr);
                return;
            }

            picQRCode.Visible = false;
        }
        catch
        {
            picQRCode.Visible = false;
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        LanguageManager.LanguageChanged -= LanguageManager_LanguageChanged;
        ThemeManager.UnregisterForm(this);
        LanguageManager.UnregisterForm(this);
        
        if (picQRCode.Image != null)
        {
            picQRCode.Image.Dispose();
            picQRCode.Image = null;
        }
        
        base.OnFormClosing(e);
    }

    private void btnOk_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }

    private void lblDonateInfo_Click(object? sender, EventArgs e)
    {
    }
    }
}

