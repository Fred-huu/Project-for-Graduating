#Region “汽车衡智能称重系统数据采集的设计与实现“

'-------------------------------------------

'导入各种库

#Region “导入数据库方面的库“
Imports MySql.Data.MySqlClient
#End Region

'-------------------------------------------

#Region “导入串口连接方面的库”
Imports System
Imports System.Threading
Imports System.IO.Ports
Imports System.ComponentModel
Imports System.IO
#End Region

'-------------------------------------------

Public Class admin

#Region “定义全局变量“
#Region “数据库”
    Dim conn As MySqlConnection
    Dim com As New MySqlCommand
    Dim dr As MySqlDataReader
    Dim data As DataTable
    Dim da As MySqlDataAdapter
#End Region

    '-------------------------------------------

#Region “串口通信”
    Dim myPort As Array
    Delegate Sub SetTextCallback(ByVal [text] As String)
#End Region
#End Region

    '-------------------------------------------

#Region “窗口界面”
    Private Sub admin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '时钟
        Timer1.Interval = 1000

        '串口连接
        myPort = IO.Ports.SerialPort.GetPortNames()
        ComboBox1.Items.AddRange(myPort)

        '设置“登录”按钮可通过回车键启动
        Me.AcceptButton = entryButton

        '定义窗口无边框
        Me.FormBorderStyle = 0

        '二级菜单
        messPanel.Visible = False
        addPanel.Visible = False
        includingPanel.Visible = False

        '一级菜单
        admin0.Visible = False
        admin1.Visible = True
        mess0.Visible = True
        mess1.Visible = False
        add0.Visible = True
        add1.Visible = False
        including0.Visible = True
        including1.Visible = False
    End Sub

    '时钟显示
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim datestr As String
        datestr = Format(Now(), "yyyy-MM-dd HH:mm:ss")
        time.Text = datestr
        logintime.Text = datestr
        logtime.Text = datestr
    End Sub

    '-------------------------------------------

    '注销账号
    Private Sub signoutLinkLabel_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles signoutLinkLabel.LinkClicked
        '定义窗口无边框
        Me.FormBorderStyle = 0

        '二级菜单
        messPanel.Visible = False
        addPanel.Visible = False
        includingPanel.Visible = False

        '清空登录界面数据
        USERTextBox.Text = ""
        PasswordTextBox.Text = ""

        'panel容器
        loginPanel.Visible = True
        newuserPanel.Visible = False
        adminPanel.Visible = False
        wPanel.Visible = False
        cmPanel.Visible = False
        dmPanel.Visible = False
        caddPanel.Visible = False
        daddPanel.Visible = False
        helpPanel.Visible = False
        suggestPanel.Visible = False
        aboutPanel.Visible = False
        backgroundPanel.Visible = False
        setupPanel.Visible = False
        menuPanel.Visible = False
        downPanel.Visible = False
    End Sub
#End Region

    '-------------------------------------------

#Region “登录界面“
    '确定按钮
    Private Sub entryButton_Click(sender As Object, e As EventArgs) Handles entryButton.Click
        '数据库操作
        If USERTextBox.Text = "" Or PasswordTextBox.Text = "" Then
            MessageBox.Show("有数据没有输入，请输入!", "错误提示!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            If Not conn Is Nothing Then conn.Close()
            Dim connStr As String
            connStr = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false",
            DataSourceTextBox.Text, UserIDTextBox.Text, PWDTextBox.Text, InitialCatalogTextBox.Text)
            Try
                conn = New MySqlConnection(connStr)
                conn.Open()
                com.Connection = conn
                com.CommandType = CommandType.Text
                com.CommandText = "Select * From users Where user_id='" & USERTextBox.Text & "' And password='" & PasswordTextBox.Text & "'"
                dr = com.ExecuteReader()
                If dr.Read() Then ' 表示有找到通过验证
                    loginPanel.Visible = False
                    menuPanel.Visible = True
                    adminPanel.Visible = True
                    downPanel.Visible = True
                    UsernameLabel.Text = dr!username & vbCrLf
                    UserLabel.Text = dr!user_id & vbCrLf
                    Me.FormBorderStyle = 1 '定义窗口有边框
                Else
                    MessageBox.Show("密码或用户名错误，请重新输入!", "错误提示!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    PasswordTextBox.Text = ""
                End If
                conn.Close()
            Catch myerror As MySqlException
                MsgBox("Error connecting to the server:" & myerror.Message)
            End Try
        End If
    End Sub

    '取消按钮
    Private Sub offButton_Click(sender As Object, e As EventArgs) Handles offButton.Click
        Me.Close()
    End Sub

    '-------------------------------------------

    '忘记密码
    Private Sub 忘记密码LinkLabel_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles 忘记密码LinkLabel.LinkClicked
        MessageBox.Show("请联系管理员找回密码！")
    End Sub

    '注册
    Private Sub 账号注册LinkLabel_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles 账号注册LinkLabel.LinkClicked
        '进入注册界面
        newuserPanel.Visible = True
        loginPanel.Visible = False

        '清除注册界面数据
        newuserTextBox.Text = ""
        newpasswTextBox.Text = ""
        newapasswTextBox.Text = ""
        newnameTextBox.Text = ""
    End Sub

    '设置数据库地址
    Private Sub setupLinkLabel_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles setupLinkLabel.LinkClicked
        'panel容器
        loginPanel.Visible = False
        adminPanel.Visible = False
        wPanel.Visible = False
        cmPanel.Visible = False
        dmPanel.Visible = False
        caddPanel.Visible = False
        daddPanel.Visible = False
        helpPanel.Visible = False
        suggestPanel.Visible = False
        aboutPanel.Visible = False
        backgroundPanel.Visible = False
        setupPanel.Visible = True
    End Sub
#End Region

    '-------------------------------------------

#Region “设置数据库地址”
    '确定按钮
    Private Sub setupOKButton_Click(sender As Object, e As EventArgs) Handles setupOKButton.Click
        setupPanel.Visible = False
        loginPanel.Visible = True
    End Sub

    '取消按钮
    Private Sub setupcancelButton_Click(sender As Object, e As EventArgs) Handles setupcancelButton.Click
        setupPanel.Visible = False
        loginPanel.Visible = True
    End Sub
#End Region

    '-------------------------------------------

#Region “账号注册界面”
    Private Sub newokButton_Click(sender As Object, e As EventArgs) Handles newokButton.Click
        '数据库操作
        If Not conn Is Nothing Then conn.Close()
        Dim connStr As String
        connStr = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false",
        DataSourceTextBox.Text, UserIDTextBox.Text, PWDTextBox.Text, InitialCatalogTextBox.Text)
        Try
            conn = New MySqlConnection(connStr)
            conn.Open()
            com.Connection = conn
            com.CommandType = CommandType.Text
            com.CommandText = "Select * From users Where user_id='" & newuserTextBox.Text & "'"
            dr = com.ExecuteReader()
            If newnameTextBox.Text = "" Or newuserTextBox.Text = "" Or newpasswTextBox.Text = "" Or newapasswTextBox.Text = "" Or newapasswTextBox.Text = "" Then
                MessageBox.Show("有数据没有输入，请输入!", "错误提示!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ElseIf dr.Read() Then ' 表示有找到通过验证
                MessageBox.Show("账号已被注册，请重新输入!", "错误提示!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                newuserTextBox.Text = ""
            Else
                If newapasswTextBox.Text <> newpasswTextBox.Text Then
                    MessageBox.Show("两次密码输入不一致，请重新输入!", "错误提示!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    insertuser()
                End If
            End If
            conn.Close()
        Catch myerror As MySqlException
            MsgBox("Error connecting to the server:" & myerror.Message)
        End Try
    End Sub

    Private Sub insertuser()
        '数据库连接与操作
        If Not conn Is Nothing Then conn.Close()
        Dim connStr As String
        connStr = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false",
        DataSourceTextBox.Text, UserIDTextBox.Text, PWDTextBox.Text, InitialCatalogTextBox.Text)
        Try
            conn = New MySqlConnection(connStr)
            conn.Open()
            com.Connection = conn
            com.CommandType = CommandType.Text
            com.CommandText = "Insert Into users (username,user_id,password,perm) Values ('" & newnameTextBox.Text & "','" & newuserTextBox.Text & "','" & newapasswTextBox.Text & "',' 0 ')"
            dr = com.ExecuteReader
            conn.Close()
            MessageBox.Show("账号注册成功！")

            '清除登录界面的数据
            USERTextBox.Text = ""
            PasswordTextBox.Text = ""

            '返回登陆界面
            loginPanel.Visible = True
            newuserPanel.Visible = False

        Catch myerror As MySqlException
            MsgBox("Error connecting to the server:" & myerror.Message)
        End Try
    End Sub

    '返回登陆界面
    Private Sub newcancelButton_Click(sender As Object, e As EventArgs) Handles newcancelButton.Click
        '清除登录界面的数据
        USERTextBox.Text = ""
        PasswordTextBox.Text = ""

        loginPanel.Visible = True
        newuserPanel.Visible = False
    End Sub
#End Region

    '-------------------------------------------

#Region “管理界面“
    '点击“管理界面”（未选中）
    Private Sub admin0_Click(sender As Object, e As EventArgs) Handles admin0.Click
        '二级菜单
        messPanel.Visible = False
        addPanel.Visible = False
        includingPanel.Visible = False

        '一级菜单
        admin0.Visible = False
        admin1.Visible = True
        mess0.Visible = True
        mess1.Visible = False
        add0.Visible = True
        add1.Visible = False
        including0.Visible = True
        including1.Visible = False

        'panel容器
        adminPanel.Visible = True
        wPanel.Visible = False
        cmPanel.Visible = False
        dmPanel.Visible = False
        caddPanel.Visible = False
        daddPanel.Visible = False
        helpPanel.Visible = False
        suggestPanel.Visible = False
        aboutPanel.Visible = False
        backgroundPanel.Visible = False
        setupPanel.Visible = False

        '隐藏修改界面
        modifyPanel.Visible = False
    End Sub

    '启动按钮
    Private Sub openButton_Click(sender As Object, e As EventArgs) Handles openButton.Click
        '二级菜单
        messPanel.Visible = False
        addPanel.Visible = False
        includingPanel.Visible = False

        '清空数据
        If TextBox1.Text <> "" And TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" And TextBox5.Text <> "" And TextBox6.Text <> "" And TextBox8.Text <> "" And TextBox10.Text <> "" And TextBox12.Text <> "" Then
            TextBox1.Text = ""
            TextBox2.Text = ""
            TextBox3.Text = ""
            TextBox4.Text = ""
            TextBox5.Text = ""
            TextBox6.Text = ""
            TextBox7.Text = ""
            TextBox8.Text = ""
            TextBox9.Text = ""
            TextBox10.Text = ""
            TextBox11.Text = ""
            TextBox12.Text = ""
            kindTextBox.Text = ""
            adminremarkTextBox.Text = ""
        End If

        '串口连接相关
        SerialPort1.PortName = ComboBox1.Text '端口
        SerialPort1.BaudRate = ComboBox2.Text '波特率
        SerialPort1.Open()

        '称量时间显示
        Dim datestr As String = ""
        datestr = Format(Now(), "yyyy-MM-dd HH:mm:ss")
        TextBox9.Text = datestr

        '数据库相关
        Try
            conn.Open()
            com.Connection = conn
            com.CommandType = CommandType.Text
            com.CommandText = "Select * From car,driver Where car.carnum='" & TextBox7.Text & "' and driver.carnum='" & TextBox7.Text & "'"
            dr = com.ExecuteReader()
            While dr.Read()
                TextBox1.AppendText(dr!driver_id & vbCrLf)
                TextBox2.AppendText(dr!name & vbCrLf)
                TextBox3.AppendText(dr!bday & vbCrLf)
                TextBox4.AppendText(dr!part & vbCrLf)
                TextBox5.AppendText(dr!dkind & vbCrLf)
                TextBox6.AppendText(dr!telnum & vbCrLf)
                TextBox8.AppendText(dr!model & vbCrLf)
                TextBox10.AppendText(dr!fload & vbCrLf)
            End While
            conn.Close()
        Catch myerror As MySqlException
            MsgBox("Error connecting to the server:" & myerror.Message)
        End Try

        '货物净重称量
        Dim a As Single
        Dim b As Single
        If (TextBox11.Text <> "" And TextBox10.Text <> "") Then
            a = TextBox11.Text
            b = TextBox10.Text
            TextBox12.Text = a - b
        Else
            TextBox12.Text = ""
        End If

        '按钮
        updateButton.Enabled = True
        openButton.Enabled = False
    End Sub

    '串口连接相关
    Sub RecieveRefreshMethod(ByVal str As String) '定义一个实例方法
        ShowRecieveData(str)
    End Sub

    Private Sub ShowRecieveData(ByVal str As String)
        On Error GoTo Err
        Exit Sub
Err:    MsgBox("数据接收或显示错误！" + vbNewLine + ErrorToString())
    End Sub

    Private Sub SerialPort1_DataReceived(sender As System.Object, e As System.IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        ReceivedText(SerialPort1.ReadExisting())
    End Sub

    Private Sub ReceivedText(ByVal [text] As String)
        Dim instr As String  '存从串口读入的数据
        Dim info() As String
        If Me.TextBox11.InvokeRequired Then
            Dim x As New SetTextCallback(AddressOf ReceivedText)
            Me.Invoke(x, New Object() {(text)})
        Else
            'Me.TextBox11.Text = [text] '录入数据

            instr = [text]
            If instr.Length <> 0 Then
                info = instr.Split("#")    '将instr以#分割
                TextBox7.Text = info(0)
                TextBox11.Text = info(1)
            End If

        End If
    End Sub

    '上传按钮
    Private Sub updateButton_Click(sender As Object, e As EventArgs) Handles updateButton.Click
        '二级菜单
        messPanel.Visible = False
        addPanel.Visible = False
        includingPanel.Visible = False

        If kindTextBox.Text = "" Then
            MessageBox.Show("请输入货物类型!", "错误提示!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            '数据库连接与操作
            Try
                conn.Open()
                com.Connection = conn
                com.CommandType = CommandType.Text
                com.CommandText = "Insert Into manage (carnum,weight,time,user_id,kind,remarks) Values ('" & TextBox7.Text & "','" & TextBox12.Text & "','" & TextBox9.Text & "','" & USERTextBox.Text & "','" & kindTextBox.Text & "','" & adminremarkTextBox.Text & "')"
                dr = com.ExecuteReader
                conn.Close()
                MsgBox("上传成功")

                '按钮
                updateButton.Enabled = False
                openButton.Enabled = True

                '关闭串口
                SerialPort1.Close()

                '清空数据
                TextBox1.Text = ""
                TextBox2.Text = ""
                TextBox3.Text = ""
                TextBox4.Text = ""
                TextBox5.Text = ""
                TextBox6.Text = ""
                TextBox7.Text = ""
                TextBox8.Text = ""
                TextBox9.Text = ""
                TextBox10.Text = ""
                TextBox11.Text = ""
                TextBox12.Text = ""
                kindTextBox.Text = ""
                adminremarkTextBox.Text = ""

            Catch myerror As MySqlException
                MsgBox("Error connecting to the server:" & myerror.Message)
            End Try
        End If

        '关闭串口
        SerialPort1.Close()
    End Sub

    '复位键
    Private Sub 复位LinkLabel_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles 复位LinkLabel.LinkClicked
        '二级菜单
        messPanel.Visible = False
        addPanel.Visible = False
        includingPanel.Visible = False

        '清除数据
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
        TextBox7.Text = ""
        TextBox8.Text = ""
        TextBox9.Text = ""
        TextBox10.Text = ""
        TextBox11.Text = ""
        TextBox12.Text = ""
        kindTextBox.Text = ""
        adminremarkTextBox.Text = ""
        ComboBox1.Text = ""
        ComboBox2.Text = ""

        '按钮
        openButton.Enabled = True
        updateButton.Enabled = False
    End Sub

    '-------------------------------------------

    '修改车牌号界面
    '打开修改界面
    Private Sub 修改LinkLabel_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles 修改LinkLabel.LinkClicked
        '二级菜单
        messPanel.Visible = False
        addPanel.Visible = False
        includingPanel.Visible = False

        modifyPanel.Visible = True

        '清空数据
        modifyComboBox.Items.Clear()

        '数据库连接与操作
        If Not conn Is Nothing Then conn.Close()
        Dim connStr As String
        connStr = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false",
        DataSourceTextBox.Text, UserIDTextBox.Text, PWDTextBox.Text, InitialCatalogTextBox.Text)
        Try
            conn = New MySqlConnection(connStr)
            conn.Open()
            com.Connection = conn
            com.CommandType = CommandType.Text
            com.CommandText = "Select carnum From car"
            dr = com.ExecuteReader()
            modifyComboBox.Items.Clear()
            While dr.Read()
                modifyComboBox.Items.Add(dr.GetString(0))
            End While
            conn.Close()
        Catch myerror As MySqlException
            MsgBox("Error connecting to the server:" & myerror.Message)
        End Try
    End Sub

    '确定修改车牌号
    Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click
        '二级菜单
        messPanel.Visible = False
        addPanel.Visible = False
        includingPanel.Visible = False

        If modifyComboBox.SelectedItem.ToString() <> "" Then
            '将修改后的车牌号传输到管理界面上
            TextBox7.Text = modifyComboBox.SelectedItem.ToString()

            '清空管理界面的值
            If TextBox1.Text <> "" And TextBox2.Text <> "" And TextBox3.Text <> "" And TextBox4.Text <> "" And TextBox5.Text <> "" And TextBox6.Text <> "" And TextBox8.Text <> "" And TextBox10.Text <> "" And TextBox12.Text <> "" Then
                TextBox1.Text = ""
                TextBox2.Text = ""
                TextBox3.Text = ""
                TextBox4.Text = ""
                TextBox5.Text = ""
                TextBox6.Text = ""
                TextBox8.Text = ""
                TextBox10.Text = ""
                TextBox12.Text = ""
                kindTextBox.Text = ""
                adminremarkTextBox.Text = ""
            End If

            '管理界面数据库操作
            If Not conn Is Nothing Then conn.Close()

            Dim connStr As String
            connStr = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false",
            DataSourceTextBox.Text, UserIDTextBox.Text, PWDTextBox.Text, InitialCatalogTextBox.Text)

            Try
                conn = New MySqlConnection(connStr)
                conn.Open()
                com.Connection = conn
                com.CommandType = CommandType.Text
                com.CommandText = "Select * From car,driver Where car.carnum='" & TextBox7.Text & "' and driver.carnum='" & TextBox7.Text & "'"
                dr = com.ExecuteReader()
                While dr.Read()
                    TextBox1.AppendText(dr!driver_id & vbCrLf)
                    TextBox2.AppendText(dr!name & vbCrLf)
                    TextBox3.AppendText(dr!bday & vbCrLf)
                    TextBox4.AppendText(dr!part & vbCrLf)
                    TextBox5.AppendText(dr!dkind & vbCrLf)
                    TextBox6.AppendText(dr!telnum & vbCrLf)
                    TextBox8.AppendText(dr!model & vbCrLf)
                    TextBox10.AppendText(dr!fload & vbCrLf)
                End While
                conn.Close()
            Catch myerror As MySqlException
                MsgBox("Error connecting to the server:" & myerror.Message)
            End Try

            '重新计算货物重量
            Dim a As Single
            Dim b As Single
            If (TextBox11.Text <> "" And TextBox10.Text <> "") Then
                a = TextBox11.Text
                b = TextBox10.Text
                TextBox12.Text = a - b
            Else
                TextBox12.Text = ""
            End If

            '隐藏修改界面
            modifyPanel.Visible = False

        Else
            '当输入数据为空时
            MessageBox.Show("输入数据为空!", "错误提示!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    '取消修改
    Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
        '二级菜单
        messPanel.Visible = False
        addPanel.Visible = False
        includingPanel.Visible = False

        modifyPanel.Visible = False
    End Sub

    '-------------------------------------------

    '后台管理
    Private Sub backgroundLinkLabel_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles backgroundLinkLabel.LinkClicked
        '一级菜单
        admin0.Visible = True
        admin1.Visible = False
        mess0.Visible = True
        mess1.Visible = False
        add0.Visible = True
        add1.Visible = False
        including0.Visible = True
        including1.Visible = False

        '二级菜单
        messPanel.Visible = False
        addPanel.Visible = False
        includingPanel.Visible = False

        'panel容器
        adminPanel.Visible = False
        wPanel.Visible = False
        cmPanel.Visible = False
        dmPanel.Visible = False
        caddPanel.Visible = False
        daddPanel.Visible = False
        helpPanel.Visible = False
        suggestPanel.Visible = False
        aboutPanel.Visible = False
        backgroundPanel.Visible = True
        setupPanel.Visible = False
    End Sub
#End Region

    '-------------------------------------------

#Region “信息查询界面”
    '点击“信息查询”（未选中）
    Private Sub mess0_Click(sender As Object, e As EventArgs) Handles mess0.Click
        '二级菜单
        If wPanel.Visible = False And cmPanel.Visible = False And dmPanel.Visible = False Then
            wmess0.Visible = True
            wmess1.Visible = False
            cmess0.Visible = True
            cmess1.Visible = False
            dmess0.Visible = True
            dmess1.Visible = False
        End If

        '二级菜单列表
        messPanel.Visible = True
        addPanel.Visible = False
        includingPanel.Visible = False

        '一级菜单
        admin0.Visible = True
        admin1.Visible = False
        mess0.Visible = False
        mess1.Visible = True
        add0.Visible = True
        add1.Visible = False
        including0.Visible = True
        including1.Visible = False
    End Sub

    '点击“信息查询”（选中）
    Private Sub mess1_Click(sender As Object, e As EventArgs) Handles mess1.Click
        If messPanel.Visible = True Then
            messPanel.Visible = False
        Else
            messPanel.Visible = True
        End If
    End Sub

    '-------------------------------------------

#Region “称量信息查询界面“
    '点击“称量信息查询界面”（未选中）
    Private Sub wmess0_Click(sender As Object, e As EventArgs) Handles wmess0.Click
        '二级菜单
        wmess0.Visible = False
        wmess1.Visible = True
        cmess0.Visible = True
        cmess1.Visible = False
        dmess0.Visible = True
        dmess1.Visible = False

        messPanel.Visible = False

        'panel容器
        adminPanel.Visible = False
        wPanel.Visible = True
        cmPanel.Visible = False
        dmPanel.Visible = False
        caddPanel.Visible = False
        daddPanel.Visible = False
        helpPanel.Visible = False
        suggestPanel.Visible = False
        aboutPanel.Visible = False
        backgroundPanel.Visible = False
        setupPanel.Visible = False

        '清空数据
        ListView1.Items.Clear()
    End Sub

    '点击“称量信息查询界面”（选中）
    Private Sub wmess1_Click(sender As Object, e As EventArgs) Handles wmess1.Click
        messPanel.Visible = False
    End Sub

    '查询按钮
    Private Sub selectButton_Click(sender As Object, e As EventArgs) Handles selectButton.Click
        '二级菜单
        messPanel.Visible = False
        addPanel.Visible = False
        includingPanel.Visible = False

        '清空数据
        ListView1.Items.Clear()

        '数据库操作
        Dim i As Integer
        Dim TABLE As New DataTable
        Dim adapter As New MySqlDataAdapter
        Dim cmd As MySqlCommand
        If Not conn Is Nothing Then conn.Close()
        Dim connStr As String
        connStr = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false",
        DataSourceTextBox.Text, UserIDTextBox.Text, PWDTextBox.Text, InitialCatalogTextBox.Text)
        Try
            conn = New MySqlConnection(connStr)
            conn.Open()
            cmd = New MySqlCommand("Select * From manage", conn)
            With adapter
                .SelectCommand = cmd
                .Fill(TABLE)
            End With
            For i = 0 To TABLE.Rows.Count - 1
                With ListView1
                    .Items.Add(TABLE.Rows(i)("manage_id"))
                    With .Items(.Items.Count - 1).SubItems
                        .Add(TABLE.Rows(i)("carnum"))
                        .Add(TABLE.Rows(i)("user_id"))
                        .Add(TABLE.Rows(i)("weight"))
                        .Add(TABLE.Rows(i)("kind"))
                        .Add(TABLE.Rows(i)("time"))
                        .Add(TABLE.Rows(i)("remarks"))
                    End With
                End With
            Next
            conn.Close()
        Catch myerror As MySqlException
            MsgBox("Error connecting to the server:" & myerror.Message)
        End Try
    End Sub
#End Region

    '-------------------------------------------

#Region “车辆信息查询界面”
    '点击“车辆信息查询界面”（未选中）
    Private Sub cmess0_Click(sender As Object, e As EventArgs) Handles cmess0.Click
        '二级菜单
        wmess0.Visible = True
        wmess1.Visible = False
        cmess0.Visible = False
        cmess1.Visible = True
        dmess0.Visible = True
        dmess1.Visible = False

        messPanel.Visible = False

        'panel容器
        adminPanel.Visible = False
        wPanel.Visible = False
        cmPanel.Visible = True
        dmPanel.Visible = False
        caddPanel.Visible = False
        daddPanel.Visible = False
        helpPanel.Visible = False
        suggestPanel.Visible = False
        aboutPanel.Visible = False
        backgroundPanel.Visible = False
        setupPanel.Visible = False

        '清空数据
        TextBox13.Text = ""
        TextBox14.Text = ""
        TextBox15.Text = ""
        TextBox17.Text = ""

        '数据库连接与操作
        Try
            conn.Open()
            com.Connection = conn
            com.CommandType = CommandType.Text
            com.CommandText = "Select carnum From car"
            dr = com.ExecuteReader()
            ccComboBox.Items.Clear()
            While dr.Read()
                ccComboBox.Items.Add(dr.GetString(0))
            End While
            conn.Close()
        Catch myerror As MySqlException
            MsgBox("Error connecting to the server:" & myerror.Message)
        End Try
    End Sub

    '点击“车辆信息查询界面”（选中）
    Private Sub cmess1_Click(sender As Object, e As EventArgs) Handles cmess1.Click
        messPanel.Visible = False
    End Sub

    '查询按钮
    Private Sub carselectButton_Click(sender As Object, e As EventArgs) Handles carselectButton.Click
        '二级菜单
        messPanel.Visible = False
        addPanel.Visible = False
        includingPanel.Visible = False

        '清除数据
        If ccComboBox.SelectedItem.ToString() <> "" Then
            TextBox13.Text = ""
            TextBox14.Text = ""
            TextBox15.Text = ""
            TextBox17.Text = ""
        End If

        '数据库连接与操作
        If Not conn Is Nothing Then conn.Close()
        Dim connStr As String
        connStr = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false",
        DataSourceTextBox.Text, UserIDTextBox.Text, PWDTextBox.Text, InitialCatalogTextBox.Text)
        Try
            conn = New MySqlConnection(connStr)
            conn.Open()
            com.Connection = conn
            com.CommandType = CommandType.Text
            com.CommandText = "Select * From car Where carnum='" & ccComboBox.SelectedItem.ToString() & "'"
            dr = com.ExecuteReader()
            If dr.Read() Then
                TextBox13.AppendText(dr!carnum & vbCrLf)
                TextBox14.AppendText(dr!model & vbCrLf)
                TextBox15.AppendText(dr!pdtime & vbCrLf)
                TextBox17.AppendText(dr!fload & vbCrLf)
            Else
                MsgBox("车牌号输错啦！  ╮(╯▽╰)╭ ")
            End If
            conn.Close()
        Catch myerror As MySqlException
            MsgBox("Error connecting to the server:" & myerror.Message)
        End Try
    End Sub
#End Region

    '-------------------------------------------

#Region “司机信息查询界面“
    '点击“司机信息查询界面”（未选中）
    Private Sub dmess0_Click(sender As Object, e As EventArgs) Handles dmess0.Click
        '二级菜单
        wmess0.Visible = True
        wmess1.Visible = False
        cmess0.Visible = True
        cmess1.Visible = False
        dmess0.Visible = False
        dmess1.Visible = True

        messPanel.Visible = False

        'panel容器
        adminPanel.Visible = False
        wPanel.Visible = False
        cmPanel.Visible = False
        dmPanel.Visible = True
        caddPanel.Visible = False
        daddPanel.Visible = False
        helpPanel.Visible = False
        suggestPanel.Visible = False
        aboutPanel.Visible = False
        backgroundPanel.Visible = False
        setupPanel.Visible = False

        '清空数据
        TextBox18.Text = ""
        TextBox19.Text = ""
        TextBox20.Text = ""
        TextBox21.Text = ""
        TextBox22.Text = ""
        TextBox23.Text = ""

        '数据库连接与操作
        Try
            conn.Open()
            com.Connection = conn
            com.CommandType = CommandType.Text
            com.CommandText = "Select driver_id From driver"
            dr = com.ExecuteReader()
            dcComboBox.Items.Clear()
            While dr.Read()
                dcComboBox.Items.Add(dr.GetString(0))
            End While
            conn.Close()
        Catch myerror As MySqlException
            MsgBox("Error connecting to the server:" & myerror.Message)
        End Try
    End Sub

    '点击“司机信息查询界面”（选中）
    Private Sub dmess1_Click(sender As Object, e As EventArgs) Handles dmess1.Click
        messPanel.Visible = False
    End Sub

    '查询按钮
    Private Sub driverselectButton_Click(sender As Object, e As EventArgs) Handles driverselectButton.Click
        '二级菜单
        messPanel.Visible = False
        addPanel.Visible = False
        includingPanel.Visible = False

        '清除数据
        If dcComboBox.SelectedItem.ToString() <> "" Then
            TextBox18.Text = ""
            TextBox19.Text = ""
            TextBox20.Text = ""
            TextBox21.Text = ""
            TextBox22.Text = ""
            TextBox23.Text = ""
        End If

        '数据库连接与操作
        If Not conn Is Nothing Then conn.Close()
        Dim connStr As String
        connStr = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false",
        DataSourceTextBox.Text, UserIDTextBox.Text, PWDTextBox.Text, InitialCatalogTextBox.Text)
        Try
            conn = New MySqlConnection(connStr)
            conn.Open()
            com.Connection = conn
            com.CommandType = CommandType.Text
            com.CommandText = "Select * From driver Where driver_id='" & dcComboBox.SelectedItem.ToString() & "'"
            dr = com.ExecuteReader()
            If dr.Read() Then
                TextBox18.AppendText(dr!driver_id & vbCrLf)
                TextBox19.AppendText(dr!name & vbCrLf)
                TextBox20.AppendText(dr!bday & vbCrLf)
                TextBox21.AppendText(dr!dkind & vbCrLf)
                TextBox22.AppendText(dr!part & vbCrLf)
                TextBox23.AppendText(dr!telnum & vbCrLf)
            Else
                MsgBox("驾驶证号输错啦！  ╮(╯▽╰)╭ ")
            End If
            conn.Close()
        Catch myerror As MySqlException
            MsgBox("Error connecting to the server:" & myerror.Message)
        End Try
    End Sub
#End Region

#End Region

    '-------------------------------------------

#Region “信息录入界面”
    '点击“信息录入界面”（未选中）
    Private Sub add0_Click(sender As Object, e As EventArgs) Handles add0.Click
        '二级菜单
        If caddPanel.Visible = False And daddPanel.Visible = False Then
            addcmess0.Visible = True
            addcmess1.Visible = False
            adddmess0.Visible = True
            adddmess1.Visible = False
        End If

        '二级菜单列表
        addPanel.Visible = True
        messPanel.Visible = False
        includingPanel.Visible = False

        '一级菜单
        admin0.Visible = True
        admin1.Visible = False
        mess0.Visible = True
        mess1.Visible = False
        add0.Visible = False
        add1.Visible = True
        including0.Visible = True
        including1.Visible = False
    End Sub

    '点击“信息录入界面”（选中）
    Private Sub add1_Click(sender As Object, e As EventArgs) Handles add1.Click
        If addPanel.Visible = True Then
            addPanel.Visible = False
        Else
            addPanel.Visible = True
        End If
    End Sub

    '-------------------------------------------

#Region “车辆信息录入界面“
    '点击“车辆信息录入界面”（未选中）
    Private Sub addcmess0_Click(sender As Object, e As EventArgs) Handles addcmess0.Click
        '二级菜单
        addcmess0.Visible = False
        addcmess1.Visible = True
        adddmess0.Visible = True
        adddmess1.Visible = False

        addPanel.Visible = False

        'panel容器
        adminPanel.Visible = False
        wPanel.Visible = False
        cmPanel.Visible = False
        dmPanel.Visible = False
        caddPanel.Visible = True
        daddPanel.Visible = False
        helpPanel.Visible = False
        suggestPanel.Visible = False
        aboutPanel.Visible = False
        backgroundPanel.Visible = False
        setupPanel.Visible = False

        '清空数据
        TextBox24.Text = ""
        TextBox25.Text = ""
        carDateTimePicker.Value = Now
        TextBox28.Text = ""

        '日历控件
        carDateTimePicker.CustomFormat = "yyyy-MM-dd"
    End Sub

    '点击“车辆信息录入界面”（选中）
    Private Sub addcmess1_Click(sender As Object, e As EventArgs) Handles addcmess1.Click
        addPanel.Visible = False
    End Sub

    '上传按钮
    Private Sub carupdateButton_Click(sender As Object, e As EventArgs) Handles carupdateButton.Click
        '二级菜单
        messPanel.Visible = False
        addPanel.Visible = False
        includingPanel.Visible = False

        '数据库连接与操作
        If Not conn Is Nothing Then conn.Close()
        Dim connStr As String
        connStr = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false",
        DataSourceTextBox.Text, UserIDTextBox.Text, PWDTextBox.Text, InitialCatalogTextBox.Text)
        Try
            conn = New MySqlConnection(connStr)
            conn.Open()
            com.Connection = conn
            com.CommandType = CommandType.Text
            com.CommandText = "Insert Into car (carnum,model,pdtime,fload) Values ('" & TextBox24.Text & "','" & TextBox25.Text & "','" & carDateTimePicker.Value & "','" & TextBox28.Text & "')"
            dr = com.ExecuteReader
            MessageBox.Show("数据存储成功！")
            conn.Close()
        Catch myerror As MySqlException
            MsgBox("Error connecting to the server:" & myerror.Message)
        End Try
    End Sub

    '清空数据
    Private Sub clearLinkLabel_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles clearLinkLabel.LinkClicked
        '二级菜单
        messPanel.Visible = False
        addPanel.Visible = False
        includingPanel.Visible = False

        '清空数据
        TextBox24.Text = ""
        TextBox25.Text = ""
        carDateTimePicker.Value = Now
        TextBox28.Text = ""
    End Sub
#End Region

    '-------------------------------------------

#Region “司机信息录入界面”
    '点击“司机信息录入界面”（未选中）
    Private Sub adddmess0_Click(sender As Object, e As EventArgs) Handles adddmess0.Click
        '二级菜单
        addcmess0.Visible = True
        addcmess1.Visible = False
        adddmess0.Visible = False
        adddmess1.Visible = True

        addPanel.Visible = False

        'panel容器
        adminPanel.Visible = False
        wPanel.Visible = False
        cmPanel.Visible = False
        dmPanel.Visible = False
        caddPanel.Visible = False
        daddPanel.Visible = True
        helpPanel.Visible = False
        suggestPanel.Visible = False
        aboutPanel.Visible = False
        backgroundPanel.Visible = False
        setupPanel.Visible = False

        '清空数据
        TextBox29.Text = ""
        TextBox30.Text = ""
        driverDateTimePicker.Value = Now
        TextBox32.Text = ""
        TextBox33.Text = ""
        TextBox34.Text = ""

        '数据库连接与操作
        Try
            conn.Open()
            com.Connection = conn
            com.CommandType = CommandType.Text
            com.CommandText = "Select carnum From car"
            dr = com.ExecuteReader()
            dacarComboBox.Items.Clear()
            While dr.Read()
                dacarComboBox.Items.Add(dr.GetString(0))
            End While
            conn.Close()
        Catch myerror As MySqlException
            MsgBox("Error connecting to the server:" & myerror.Message)
        End Try

        '日历控件
        driverDateTimePicker.CustomFormat = "yyyy-MM-dd"
    End Sub

    '点击“司机信息录入界面”（选中）
    Private Sub adddmess1_Click(sender As Object, e As EventArgs) Handles adddmess1.Click
        addPanel.Visible = False
    End Sub

    '上传按钮
    Private Sub driverupdateButton_Click(sender As Object, e As EventArgs) Handles driverupdateButton.Click
        '二级菜单
        messPanel.Visible = False
        addPanel.Visible = False
        includingPanel.Visible = False

        '数据库连接与操作
        If Not conn Is Nothing Then conn.Close()
        Dim connStr As String
        connStr = String.Format("server={0};user id={1}; password={2}; database={3}; pooling=false",
        DataSourceTextBox.Text, UserIDTextBox.Text, PWDTextBox.Text, InitialCatalogTextBox.Text)
        Try
            conn = New MySqlConnection(connStr)
            conn.Open()
            com.Connection = conn
            com.CommandType = CommandType.Text
            com.CommandText = "Insert Into driver (driver_id,name,bday,dkind,part,telnum,carnum) Values ('" & TextBox29.Text & "','" & TextBox30.Text & "','" & driverDateTimePicker.Value & "','" & TextBox32.Text & "','" & TextBox33.Text & "','" & TextBox34.Text & "','" & dacarComboBox.SelectedItem.ToString() & "')"
            dr = com.ExecuteReader
            MessageBox.Show("存储成功！")
            conn.Close()
        Catch myerror As MySqlException
            MsgBox("Error connecting to the server:" & myerror.Message)
        End Try
    End Sub

    '清空数据
    Private Sub clearLinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles clearLinkLabel2.LinkClicked
        '二级菜单
        messPanel.Visible = False
        addPanel.Visible = False
        includingPanel.Visible = False

        '清空数据
        TextBox29.Text = ""
        TextBox30.Text = ""
        driverDateTimePicker.Value = Now
        TextBox32.Text = ""
        TextBox33.Text = ""
        TextBox34.Text = ""

        '数据库连接与操作
        Try
            conn.Open()
            com.Connection = conn
            com.CommandType = CommandType.Text
            com.CommandText = "Select carnum From car"
            dr = com.ExecuteReader()
            dacarComboBox.Items.Clear()
            While dr.Read()
                dacarComboBox.Items.Add(dr.GetString(0))
            End While
            conn.Close()
        Catch myerror As MySqlException
            MsgBox("Error connecting to the server:" & myerror.Message)
        End Try
    End Sub
#End Region
#End Region

    '-------------------------------------------

#Region “系统说明界面“
    '点击“系统说明界面”（未选中）
    Private Sub including0_Click(sender As Object, e As EventArgs) Handles including0.Click
        '二级菜单
        If helpPanel.Visible = False And suggestPanel.Visible = False And aboutPanel.Visible = False Then
            help0.Visible = True
            help1.Visible = False
            suggest0.Visible = True
            suggest1.Visible = False
            about0.Visible = True
            about1.Visible = False
        End If

        '二级菜单列表
        messPanel.Visible = False
        addPanel.Visible = False
        includingPanel.Visible = True

        '一级菜单
        admin0.Visible = True
        admin1.Visible = False
        mess0.Visible = True
        mess1.Visible = False
        add0.Visible = True
        add1.Visible = False
        including0.Visible = False
        including1.Visible = True
    End Sub

    '点击“系统说明界面”（选中）
    Private Sub including1_Click(sender As Object, e As EventArgs) Handles including1.Click
        If includingPanel.Visible = True Then
            includingPanel.Visible = False
        Else
            includingPanel.Visible = True
        End If
    End Sub

    '-------------------------------------------

#Region “使用帮助界面”
    '点击“使用帮助界面”（未选中）
    Private Sub help0_Click(sender As Object, e As EventArgs) Handles help0.Click
        '二级菜单
        help0.Visible = False
        help1.Visible = True
        suggest0.Visible = True
        suggest1.Visible = False
        about0.Visible = True
        about1.Visible = False

        includingPanel.Visible = False

        'panel容器
        adminPanel.Visible = False
        wPanel.Visible = False
        cmPanel.Visible = False
        dmPanel.Visible = False
        caddPanel.Visible = False
        daddPanel.Visible = False
        helpPanel.Visible = True
        suggestPanel.Visible = False
        aboutPanel.Visible = False
        backgroundPanel.Visible = False
        setupPanel.Visible = False
    End Sub

    '点击“使用帮助界面”（选中）
    Private Sub help1_Click(sender As Object, e As EventArgs) Handles help1.Click
        includingPanel.Visible = False
    End Sub
#End Region

    '-------------------------------------------

#Region “意见反馈界面“
    '点击“意见反馈界面”（未选中）
    Private Sub suggest0_Click(sender As Object, e As EventArgs) Handles suggest0.Click
        '二级菜单
        help0.Visible = True
        help1.Visible = False
        suggest0.Visible = False
        suggest1.Visible = True
        about0.Visible = True
        about1.Visible = False

        includingPanel.Visible = False

        'panel容器
        adminPanel.Visible = False
        wPanel.Visible = False
        cmPanel.Visible = False
        dmPanel.Visible = False
        caddPanel.Visible = False
        daddPanel.Visible = False
        helpPanel.Visible = False
        suggestPanel.Visible = True
        aboutPanel.Visible = False
        backgroundPanel.Visible = False
        setupPanel.Visible = False

        '清空数据
        suggestTextBox.Text = ""
    End Sub

    '点击“意见反馈界面”（选中）
    Private Sub suggest1_Click(sender As Object, e As EventArgs) Handles suggest1.Click
        includingPanel.Visible = False
    End Sub

    '发送邮件
    Private Sub suggestButton_Click(sender As Object, e As EventArgs) Handles suggestButton.Click
        Dim smtp As New System.Net.Mail.SmtpClient("smtp.163.com", 25) '创建发件连接,邮箱的SMTP设置填充
        smtp.Credentials = New System.Net.NetworkCredential("huu_007@163.com", "huan19931224") '发件邮箱身份验证,发件邮箱登录名和密码
        Dim mail As New System.Net.Mail.MailMessage() '创建邮件
        mail.Subject = "意见反馈" '主题
        mail.SubjectEncoding = System.Text.Encoding.GetEncoding("GB2312") '主题编码
        mail.BodyEncoding = System.Text.Encoding.GetEncoding("GB2312") '正文件编码
        mail.From = New System.Net.Mail.MailAddress("huu_007@163.com") '发件人邮箱
        mail.Priority = System.Net.Mail.MailPriority.Normal '邮件优先级
        mail.IsBodyHtml = True 'HTML格式的邮件,为false则发送纯文本邮箱
        mail.Body = suggestTextBox.Text '邮件内容
        mail.To.Add("369964831@qq.com") '添加收件人
        '发送邮件
        Try
            smtp.Send(mail)
            MessageBox.Show("发送成功")
            suggestTextBox.Text = "" '发送完成清除建议框中的内容
        Catch
            MessageBox.Show("发送失败")
        Finally
            mail.Dispose()
        End Try
    End Sub
#End Region

    '-------------------------------------------

#Region “关于界面”
    '点击“关于界面”（未选中）
    Private Sub about0_Click(sender As Object, e As EventArgs) Handles about0.Click
        '二级菜单
        help0.Visible = True
        help1.Visible = False
        suggest0.Visible = True
        suggest1.Visible = False
        about0.Visible = False
        about1.Visible = True

        includingPanel.Visible = False

        'panel容器
        adminPanel.Visible = False
        wPanel.Visible = False
        cmPanel.Visible = False
        dmPanel.Visible = False
        caddPanel.Visible = False
        daddPanel.Visible = False
        helpPanel.Visible = False
        suggestPanel.Visible = False
        aboutPanel.Visible = True
        backgroundPanel.Visible = False
        setupPanel.Visible = False
    End Sub

    '点击“关于界面”（选中）
    Private Sub about1_Click(sender As Object, e As EventArgs) Handles about1.Click
        includingPanel.Visible = False
    End Sub

#End Region
#End Region

    '-------------------------------------------

End Class
#End Region