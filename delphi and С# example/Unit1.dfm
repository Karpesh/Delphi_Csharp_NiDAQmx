object Form1: TForm1
  Left = 13
  Top = 27
  Caption = 'Form1'
  ClientHeight = 636
  ClientWidth = 470
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  Position = poDesigned
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 124
    Top = 293
    Width = 31
    Height = 13
    Caption = 'Label1'
  end
  object Label2: TLabel
    Left = 303
    Top = 292
    Width = 31
    Height = 13
    Caption = 'Label1'
  end
  object Label4: TLabel
    Left = 303
    Top = 323
    Width = 31
    Height = 13
    Caption = 'Label1'
  end
  object Label5: TLabel
    Left = 335
    Top = 396
    Width = 31
    Height = 13
    Caption = 'Label5'
  end
  object Label6: TLabel
    Left = 335
    Top = 415
    Width = 31
    Height = 13
    Caption = 'Label6'
  end
  object Button1: TButton
    Left = 8
    Top = 360
    Width = 121
    Height = 25
    Caption = 'Dig_Out_line: true'
    TabOrder = 0
    OnClick = Button1Click
  end
  object Button2: TButton
    Left = 8
    Top = 391
    Width = 121
    Height = 25
    Caption = 'Dig_Out_line: false'
    TabOrder = 1
    OnClick = Button2Click
  end
  object Button3: TButton
    Left = 8
    Top = 287
    Width = 110
    Height = 25
    Caption = 'Analog Cont In start'
    TabOrder = 2
    OnClick = Button3Click
  end
  object Button4: TButton
    Left = 8
    Top = 318
    Width = 110
    Height = 25
    Caption = 'Analog Cont In stop'
    TabOrder = 3
    OnClick = Button4Click
  end
  object Chart1: TChart
    Left = 0
    Top = 0
    Width = 470
    Height = 281
    Legend.Visible = False
    Title.Text.Strings = (
      'TChart')
    LeftAxis.Automatic = False
    LeftAxis.AutomaticMaximum = False
    LeftAxis.AutomaticMinimum = False
    LeftAxis.Maximum = 10.000000000000000000
    LeftAxis.Minimum = -10.000000000000000000
    View3D = False
    Align = alTop
    TabOrder = 4
    ColorPaletteIndex = 13
    object Label3: TLabel
      Left = 16
      Top = 12
      Width = 31
      Height = 13
      Caption = 'Label1'
    end
    object Series1: TLineSeries
      Marks.Arrow.Visible = True
      Marks.Callout.Brush.Color = clBlack
      Marks.Callout.Arrow.Visible = True
      Marks.Visible = False
      SeriesColor = clRed
      Pointer.InflateMargins = True
      Pointer.Style = psRectangle
      Pointer.Visible = False
      XValues.Name = 'X'
      XValues.Order = loAscending
      YValues.Name = 'Y'
      YValues.Order = loNone
    end
    object Series2: TLineSeries
      Marks.Arrow.Visible = True
      Marks.Callout.Brush.Color = clBlack
      Marks.Callout.Arrow.Visible = True
      Marks.Visible = False
      Pointer.InflateMargins = True
      Pointer.Style = psRectangle
      Pointer.Visible = False
      XValues.Name = 'X'
      XValues.Order = loAscending
      YValues.Name = 'Y'
      YValues.Order = loNone
    end
  end
  object Button5: TButton
    Left = 8
    Top = 511
    Width = 89
    Height = 25
    Caption = 'AO_Vwrite: 5 V'
    TabOrder = 5
    OnClick = Button5Click
  end
  object Button6: TButton
    Left = 8
    Top = 542
    Width = 89
    Height = 25
    Caption = 'AO_Vwrite: 3 V'
    TabOrder = 6
    OnClick = Button6Click
  end
  object Button9: TButton
    Left = 8
    Top = 433
    Width = 121
    Height = 25
    Caption = 'Start geteration wave'
    TabOrder = 7
    OnClick = Button9Click
  end
  object Button10: TButton
    Left = 8
    Top = 464
    Width = 121
    Height = 25
    Caption = 'Stop geteration wave'
    TabOrder = 8
    OnClick = Button10Click
  end
  object Button7: TButton
    Left = 208
    Top = 287
    Width = 89
    Height = 25
    Caption = 'AI_VRead: '
    TabOrder = 9
    OnClick = Button7Click
  end
  object Button8: TButton
    Left = 208
    Top = 318
    Width = 89
    Height = 25
    Caption = 'Dig_In_line:'
    TabOrder = 10
    OnClick = Button8Click
  end
  object Button11: TButton
    Left = 216
    Top = 392
    Width = 75
    Height = 25
    Caption = 'Start timer'
    TabOrder = 11
    OnClick = Button11Click
  end
  object Button12: TButton
    Left = 216
    Top = 423
    Width = 75
    Height = 25
    Caption = 'Stop timer'
    TabOrder = 12
    OnClick = Button12Click
  end
  object Timer1: TTimer
    Enabled = False
    Interval = 80
    OnTimer = Timer1Timer
    Left = 408
    Top = 328
  end
  object MyNiDAQmx1: TMyNiDAQmx
    AutoConnect = False
    ConnectKind = ckRunningOrNew
    OnAI_DataReceived = MyNiDAQmx1AI_DataReceived
    Left = 200
    Top = 184
  end
end
