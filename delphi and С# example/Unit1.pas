unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, OleServer, mscorlib_TLB, dotnetNiDAQmx_TLB, TeEngine,
  Series, ExtCtrls, TeeProcs, Chart, GPCTRDAQmx_TLB, Activex, math;

type
  TForm1 = class(TForm)
    Button1: TButton;
    Button2: TButton;
    Button3: TButton;
    Label1: TLabel;
    Button4: TButton;
    Chart1: TChart;
    Series1: TLineSeries;
    Button5: TButton;
    Button6: TButton;
    Timer1: TTimer;
    Label2: TLabel;
    Series2: TLineSeries;
    Label3: TLabel;
    MyNiDAQmx1: TMyNiDAQmx;
    Button9: TButton;
    Button10: TButton;
    Button7: TButton;
    Button8: TButton;
    Label4: TLabel;
    Button11: TButton;
    Button12: TButton;
    Label5: TLabel;
    Label6: TLabel;
    procedure Button1Click(Sender: TObject);
    procedure Button2Click(Sender: TObject);
    procedure Button3Click(Sender: TObject);
    procedure Button4Click(Sender: TObject);

    procedure Button5Click(Sender: TObject);
    procedure Button6Click(Sender: TObject);
    procedure MyNiDAQmx1AI_DataReceived(ASender: TObject; ScaledData,
      sampleCounts: OleVariant; chanCount: Integer);
    procedure Button9Click(Sender: TObject);
    procedure Button10Click(Sender: TObject);
    procedure Button7Click(Sender: TObject);
    procedure Button8Click(Sender: TObject);
    procedure Button11Click(Sender: TObject);
    procedure Button12Click(Sender: TObject);
    procedure Timer1Timer(Sender: TObject);

  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;
  cyc: integer;





implementation

{$R *.dfm}



procedure TForm1.Button11Click(Sender: TObject);
begin
timer1.Enabled:=true;
end;

procedure TForm1.Button12Click(Sender: TObject);
begin
timer1.Enabled:=false;
end;

procedure TForm1.Button1Click(Sender: TObject);
begin
MyNiDAQmx1.Dig_Out_Line(1,0,0,1);
MyNiDAQmx1.Dig_Out_Line(1,0,1,1);
end;

procedure TForm1.Button2Click(Sender: TObject);
begin
MyNiDAQmx1.Dig_Out_Line(1,0,0,0);
MyNiDAQmx1.Dig_Out_Line(1,0,1,0);
end;



procedure TForm1.Button3Click(Sender: TObject);
begin
MyNiDAQmx1.AI_sampleRate:=100;
MyNiDAQmx1.AI_samplesPerChannel:=10;
MyNiDAQmx1.AI_ChanelString:='0,1';
MyNiDAQmx1.AI_Start;
end;

procedure TForm1.Button4Click(Sender: TObject);
begin
MyNiDAQmx1.AI_Stop;
end;

procedure TForm1.Button5Click(Sender: TObject);
begin
MyNiDAQmx1.AO_VWrite(1,0,5);
end;

procedure TForm1.Button6Click(Sender: TObject);
begin
MyNiDAQmx1.AO_VWrite(1,0,3);
end;

procedure TForm1.Button7Click(Sender: TObject);
var data: double;
begin
MyNiDAQmx1.AI_VRead(1,0,-1,data);
label2.Caption:='ACH0: '+floattostr(data)+' V';
end;

procedure TForm1.Button8Click(Sender: TObject);
var state: smallint;
begin
MyNiDAQmx1.Dig_In_Line(1,0,4,state);
label4.Caption:='DI 4: '+inttostr(state);
end;

procedure TForm1.Button9Click(Sender: TObject);

var V: OleVariant;
    data: Psafearray;
    i: integer;

begin
  V:=VarArrayCreate([0,1, 0,99], varDouble);
  data:=PSafeArray(TVarData(V).VArray);

  for i:=0 to 99 do v[0,i]:=10+0/100-10*(1/pi*arccos(0.8*sin(2*pi*i/100)));
  for i:=0 to 99 do v[1,i]:=10+0/100-10*(1/pi*arccos(-0.8*sin(2*pi*i/100)));


MyNiDAQmx1.Wave_Start('Dev1/ao0,Dev1/ao1',-10,10,100,5,data);

end;


procedure TForm1.Button10Click(Sender: TObject);
begin
MyNiDAQmx1.Wave_Stop;
end;


procedure TForm1.MyNiDAQmx1AI_DataReceived(ASender: TObject; ScaledData,
  sampleCounts: OleVariant; chanCount: Integer);

  var i: integer;

  begin

  series1.Clear;
  series2.Clear;
  for i := 0 to samplecounts[0] - 1 do
  begin
  series1.AddXY(i,scaleddata[0,i]);
  series2.AddXY(i,ScaledData[1,i]);
  end;

 inc(cyc);
 label3.Caption:='channels: '+inttostr(chanCount)+', samples: '+inttostr(sampleCounts[0]);
 label1.Caption:=inttostr(cyc);
end;

procedure TForm1.Timer1Timer(Sender: TObject);
var volt: double;
begin
with MyNiDAQmx1 do begin
AO_VWrite(1,0,5);
DIG_Out_Line(1,0,0,1);
DIG_Out_Line(1,0,0,0);
AI_VRead(1,0,1,volt);
label5.Caption:=floattostr(volt);
AI_VRead(1,1,1,volt);
label6.Caption:=floattostr(volt);
end;
end;


end.
