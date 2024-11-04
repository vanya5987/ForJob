//��������� �����������...

private void checkButton_Click(object sender, EventArgs e)
{
    if (this.passportTextbox.Text.Trim() == "")
    {
        int num1 = (int)MessageBox.Show("������� ����� � ����� ��������");
    }
    else
    {
        string rawData = this.passportTextbox.Text.Trim().Replace(" ", string.Empty);
        if (rawData.Length < 10)
        {
            this.textResult.Text = "�������� ������ ����� ��� ������ ��������";
        }
        else
        {
            string commandText = string.Format("select * from passports where num='{0}' limit 1;", (object)Form1.ComputeSha256Hash(rawData));
            string connectionString = string.Format("Data Source=" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\db.sqlite");
            try
            {
                SQLiteConnection connection = new SQLiteConnection(connectionString);
                connection.Open();
                SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter(new SQLiteCommand(commandText, connection));
                DataTable dataTable1 = new DataTable();
                DataTable dataTable2 = dataTable1;
                sqLiteDataAdapter.Fill(dataTable2);
                if (dataTable1.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dataTable1.Rows[0].ItemArray[1]))
                        this.textResult.Text = "�� �������� �" + this.passportTextbox.Text + "� ������ � ��������� �� ������������� ����������� ����������� ������������";
                    else
                        this.textResult.Text = "�� �������� �" + this.passportTextbox.Text + "� ������ � ��������� �� ������������� ����������� ����������� �� ��������������";
                }
                else
                    this.textResult.Text = "������� �" + this.passportTextbox.Text + "� � ������ ���������� �������������� ����������� �� ������";
                connection.Close();
            }
            catch (SQLiteException ex)
            {
                if (ex.ErrorCode != 1)
                    return;
                int num2 = (int)MessageBox.Show("���� db.sqlite �� ������. �������� ���� � ����� ������ � exe.");
            }
        }
    }
}