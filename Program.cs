using System;
using Gtk;

namespace XtremeTacToe
{

	class MainClass
	{
		static private int playerTurnCounter = 0;
		public static void Main ()
		{
			Application.Init ();

			//Create the Window
			Window myWin = new Window("XtremeTacToe");
			myWin.Resize(800,800);

//			Gdk.Color col = new Gdk.Color ();
//			Gdk.Color.Parse ("blue", ref col);
//
//			myWin.ModifyBg (StateType.Normal, col);

			Table myTable = (Table) MakeOuterTable ();
			myTable.RowSpacing = 6;
			myTable.ColumnSpacing = 6;

			myWin.Add (myTable);

			//Show Everything
			myWin.ShowAll();

			Application.Run();
		}

		public static Widget MakeOuterTable()
		{

			// Create a table with 3 rows and 3 column
			Table tableLayout = new Table(3, 3, true);
			//Label gameLabel = new Label("TTT Game");

			//tableLayout.RowSpacing = 20;
			//tableLayout.ColumnSpacing = 20;

			Frame vbox1 = createVBox ();
			Frame vbox2 = createVBox ();
			Frame vbox3 = createVBox ();
			Frame vbox4 = createVBox ();
			Frame vbox5 = createVBox ();
			Frame vbox6 = createVBox ();
			Frame vbox7 = createVBox ();
			Frame vbox8 = createVBox ();
			Frame vbox9 = createVBox ();



			tableLayout.Attach(vbox1 ,   0, 1, 0, 1);
			tableLayout.Attach(vbox2 ,   1, 2, 0, 1);
			tableLayout.Attach(vbox3,   2, 3, 0, 1);

			tableLayout.Attach(vbox4,   0, 1, 1, 2);
			tableLayout.Attach(vbox5,   1, 2, 1, 2);
			tableLayout.Attach(vbox6,   2, 3, 1, 2);

			tableLayout.Attach(vbox7,   0, 1, 2, 3);
			tableLayout.Attach(vbox8,   1, 2, 2, 3);
			tableLayout.Attach(vbox9,   2, 3, 2, 3);
		
			tableLayout.ShowAll();
			return tableLayout;
		}

		static void AddButton(HBox box){
			Button curButton = new Button ();
			curButton.Clicked += buttonCallback;


			Frame frame = new Frame ();
			frame.Add (curButton);

			Gdk.Color col = new Gdk.Color ();
			Gdk.Color.Parse ("red", ref col);

			frame.ModifyBg (StateType.Normal, col);


			box.PackStart (frame, true, true, 0);
			box.Show ();
		}

		static void buttonCallback(object obj, EventArgs args){
			playerTurnCounter++;
			Button button = (Button)obj;

			if (playerTurnCounter % 2 == 0)
				button.Label = "X";
			else
				button.Label = "O";
		}

		static void AddHBox(VBox box, HBox box2){
			box.PackStart (box2 , true, true, 0);
		}

		static Frame createVBox(){
			HBox myBox1 = new HBox ();

			AddButton (myBox1);
			AddButton (myBox1);
			AddButton (myBox1);

			HBox myBox2 = new HBox ();

			AddButton (myBox2);
			AddButton (myBox2);
			AddButton (myBox2);

			HBox myBox3 = new HBox ();

			AddButton (myBox3);
			AddButton (myBox3);
			AddButton (myBox3);




			VBox myVbox = new VBox ();

			AddHBox (myVbox, myBox1);
			AddHBox (myVbox, myBox2);
			AddHBox (myVbox, myBox3);


			Frame frame = new Frame ();
			frame.Add (myVbox);

			Gdk.Color col = new Gdk.Color ();
			Gdk.Color.Parse ("blue", ref col);

			frame.ModifyBg (StateType.Normal, col);
			//frame.BorderWidth = 5;

			return frame;
		}
			
	}

	class GameGrader{

		public GameGrader(){

		}

	}
}
