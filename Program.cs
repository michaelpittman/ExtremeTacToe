using System;
using Gtk;
using System.Collections;
using GLib;

namespace XtremeTacToe
{

	class MainClass: IDisposable
	{

		//Global variable for myTable
		static Table myTable;

		//Arrays that hold the contents of each game
		//For all games the index corresponds to the button in the game and the move 
		//made for that spot is as follows: 0=empty 1=X 2=O
		static int[] gameArray1 = new int[9];
		static int[] gameArray2 = new int[9];
		static int[] gameArray3 = new int[9];
		static int[] gameArray4 = new int[9];
		static int[] gameArray5 = new int[9];
		static int[] gameArray6 = new int[9];
		static int[] gameArray7 = new int[9];
		static int[] gameArray8 = new int[9];
		static int[] gameArray9 = new int[9];
		static int[] wholeGameArray = new int[9];

		//array to show which buttons have been clicked
		static int[] buttonsClicked = new int[81];

		//Frame's for all of the sectors
		static Frame vbox1;
		static Frame vbox2;
		static Frame vbox3;
		static Frame vbox4;
		static Frame vbox5;
		static Frame vbox6;
		static Frame vbox7;
		static Frame vbox8;
		static Frame vbox9;


		// 0 = all sectors open, 1-9 = sector number currently active
		static int sector = 0;

		//counter to keep track of which players' turn it is
		static private int playerTurnCounter = 0;

		//counter for button creation to assign 
		static int buttonCreatedCounter = 1;

		//sector has been won and updated flag
		static int[] sectorFlags = new int[9];

		//global variable for entire window
		static Window myWin;

		public static void Main ()
		{
			Application.Init ();

			//Create the Window
			myWin = new Window("XtremeTacToe");
			myWin.Resize(800,800);

			Gdk.Color col = new Gdk.Color (40,40,40);

			myWin.ModifyBg (StateType.Normal, col);

			initializeGameArrayToZero (gameArray1);
			initializeGameArrayToZero (gameArray2);
			initializeGameArrayToZero (gameArray3);
			initializeGameArrayToZero (gameArray4);
			initializeGameArrayToZero (gameArray5);
			initializeGameArrayToZero (gameArray6);
			initializeGameArrayToZero (gameArray7);
			initializeGameArrayToZero (gameArray8);
			initializeGameArrayToZero (gameArray9);
			initializeGameArrayToZero (wholeGameArray);

			initializeGameArrayToZero (sectorFlags);

			initializeButtonsClickedToZero (buttonsClicked);

			myTable = (Table) MakeOuterTable ();
			myTable.RowSpacing = 6;
			myTable.ColumnSpacing = 6;

			myWin.Add (myTable);

			//Show Everything
			myWin.ShowAll();

			Application.Run();
		}

		public static void initializeGameArrayToZero(int[] array){
			int i;
			for(i=0; i<9; i++){
				array[i] = 0;
			}
		}

		public static void initializeButtonsClickedToZero(int[] array){
			int i;
			for (i = 0; i < 81; i++) {
				array [i] = 0;
			}
		}

		//returns 1 if sector is full
		//returns 0 otherwise
		static int checkIfSectorIsFull(int sectorToCheck){

			int variable = (sectorToCheck - 1) * 9;

				int flag = 1;
				int i;
				for (i = variable; i < variable+9; i++) {
					if (buttonsClicked [i] == 0)
						flag = 0;
				}

				if (flag == 1) {
					return 1;
				} else
					return 0;
			
		}

		public static Widget MakeOuterTable()
		{

			// Create a table with 3 rows and 3 column
			Table tableLayout = new Table(3, 3, true);
			//Label gameLabel = new Label("TTT Game");

			//tableLayout.RowSpacing = 20;
			//tableLayout.ColumnSpacing = 20;

			vbox1 = createVBox ();
			vbox2 = createVBox ();
			vbox3 = createVBox ();
			vbox4 = createVBox ();
			vbox5 = createVBox ();
			vbox6 = createVBox ();
			vbox7 = createVBox ();
			vbox8 = createVBox ();
			vbox9 = createVBox ();



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
			buttonCallbackAssigner (curButton);
			buttonCreatedCounter++;


			Frame frame = new Frame ();
			frame.Add (curButton);

			Gdk.Color col = new Gdk.Color ();
			Gdk.Color.Parse ("green", ref col);

			frame.ModifyBg (StateType.Normal, col);


			box.PackStart (frame, true, true, 0);
			box.Show ();
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

			Gdk.Color col = new Gdk.Color (40,40,40);
			//Gdk.Color.Parse ("blue", ref col);

			frame.ModifyBg (StateType.Normal, col);
			//frame.BorderWidth = 5;

			return frame;
		}

		static void highlightIndividualSectorGreen(Frame vbox){
			Gdk.Color col = new Gdk.Color ();
			Gdk.Color.Parse ("green", ref col);

			VBox thisVbox = (VBox) vbox.Child;
			var enumerable = thisVbox.AllChildren;
			IEnumerator sequenceEnum = enumerable.GetEnumerator ();
			while (sequenceEnum.MoveNext ()) {
				HBox thisHbox = (HBox) sequenceEnum.Current;
				var hEnumerable = thisHbox.AllChildren;
				IEnumerator seqEnum = hEnumerable.GetEnumerator ();
					while (seqEnum.MoveNext ()) {
						Frame buttonFrame = (Frame) seqEnum.Current;
						buttonFrame.ModifyBg (StateType.Normal, col);
					}

			}

		}

		static void highlightIndividualSectorRed(Frame vbox){
			Gdk.Color col = new Gdk.Color ();
			Gdk.Color.Parse ("red", ref col);

			VBox thisVbox = (VBox) vbox.Child;
			var enumerable = thisVbox.AllChildren;
			IEnumerator sequenceEnum = enumerable.GetEnumerator ();
			while (sequenceEnum.MoveNext ()) {
				HBox thisHbox = (HBox) sequenceEnum.Current;
				var hEnumerable = thisHbox.AllChildren;
				IEnumerator seqEnum = hEnumerable.GetEnumerator ();
				while (seqEnum.MoveNext ()) {
					Frame buttonFrame = (Frame) seqEnum.Current;
					buttonFrame.ModifyBg (StateType.Normal, col);
				}

			}
		}

		static void highlightSector(){
			if (sector == 0) {
				if(wholeGameArray[0] == 0)
					highlightIndividualSectorGreen (vbox1);
				if(wholeGameArray[1] == 0)
					highlightIndividualSectorGreen (vbox2);
				if(wholeGameArray[2] == 0)
					highlightIndividualSectorGreen (vbox3);
				if(wholeGameArray[3] == 0)
					highlightIndividualSectorGreen (vbox4);
				if(wholeGameArray[4] == 0)
					highlightIndividualSectorGreen (vbox5);
				if(wholeGameArray[5] == 0)
					highlightIndividualSectorGreen (vbox6);
				if(wholeGameArray[6] == 0)
					highlightIndividualSectorGreen (vbox7);
				if(wholeGameArray[7] == 0)
					highlightIndividualSectorGreen (vbox8);
				if(wholeGameArray[8] == 0)
					highlightIndividualSectorGreen (vbox9);
			} else if (sector == 1) {
				if(wholeGameArray[0] == 0)
					highlightIndividualSectorGreen (vbox1);
				if(wholeGameArray[1] == 0)
					highlightIndividualSectorRed (vbox2);
				if(wholeGameArray[2] == 0)
					highlightIndividualSectorRed (vbox3);
				if(wholeGameArray[3] == 0)
					highlightIndividualSectorRed (vbox4);
				if(wholeGameArray[4] == 0)
					highlightIndividualSectorRed (vbox5);
				if(wholeGameArray[5] == 0)
					highlightIndividualSectorRed (vbox6);
				if(wholeGameArray[6] == 0)
					highlightIndividualSectorRed (vbox7);
				if(wholeGameArray[7] == 0)
					highlightIndividualSectorRed (vbox8);
				if(wholeGameArray[8] == 0)
					highlightIndividualSectorRed (vbox9);
			} else if (sector == 2) {
				if(wholeGameArray[0] == 0)
					highlightIndividualSectorRed (vbox1);
				if(wholeGameArray[1] == 0)
					highlightIndividualSectorGreen (vbox2);
				if(wholeGameArray[2] == 0)
					highlightIndividualSectorRed (vbox3);
				if(wholeGameArray[3] == 0)
					highlightIndividualSectorRed (vbox4);
				if(wholeGameArray[4] == 0)
					highlightIndividualSectorRed (vbox5);
				if(wholeGameArray[5] == 0)
					highlightIndividualSectorRed (vbox6);
				if(wholeGameArray[6] == 0)
					highlightIndividualSectorRed (vbox7);
				if(wholeGameArray[7] == 0)
					highlightIndividualSectorRed (vbox8);
				if(wholeGameArray[8] == 0)
					highlightIndividualSectorRed (vbox9);
			} else if (sector == 3) {
				if(wholeGameArray[0] == 0)
					highlightIndividualSectorRed (vbox1);
				if(wholeGameArray[1] == 0)
					highlightIndividualSectorRed (vbox2);
				if(wholeGameArray[2] == 0)
					highlightIndividualSectorGreen (vbox3);
				if(wholeGameArray[3] == 0)
					highlightIndividualSectorRed (vbox4);
				if(wholeGameArray[4] == 0)
					highlightIndividualSectorRed (vbox5);
				if(wholeGameArray[5] == 0)
					highlightIndividualSectorRed (vbox6);
				if(wholeGameArray[6] == 0)
					highlightIndividualSectorRed (vbox7);
				if(wholeGameArray[7] == 0)
					highlightIndividualSectorRed (vbox8);
				if(wholeGameArray[8] == 0)
					highlightIndividualSectorRed (vbox9);
			} else if (sector == 4) {
				if(wholeGameArray[0] == 0)
					highlightIndividualSectorRed (vbox1);
				if(wholeGameArray[1] == 0)
					highlightIndividualSectorRed (vbox2);
				if(wholeGameArray[2] == 0)
					highlightIndividualSectorRed (vbox3);
				if(wholeGameArray[3] == 0)
					highlightIndividualSectorGreen (vbox4);
				if(wholeGameArray[4] == 0)
					highlightIndividualSectorRed (vbox5);
				if(wholeGameArray[5] == 0)
					highlightIndividualSectorRed (vbox6);
				if(wholeGameArray[6] == 0)
					highlightIndividualSectorRed (vbox7);
				if(wholeGameArray[7] == 0)
					highlightIndividualSectorRed (vbox8);
				if(wholeGameArray[8] == 0)
					highlightIndividualSectorRed (vbox9);
			} else if (sector == 5) {
				if(wholeGameArray[0] == 0)
					highlightIndividualSectorRed (vbox1);
				if(wholeGameArray[1] == 0)
					highlightIndividualSectorRed (vbox2);
				if(wholeGameArray[2] == 0)
					highlightIndividualSectorRed (vbox3);
				if(wholeGameArray[3] == 0)
					highlightIndividualSectorRed (vbox4);
				if(wholeGameArray[4] == 0)
					highlightIndividualSectorGreen (vbox5);
				if(wholeGameArray[5] == 0)
					highlightIndividualSectorRed (vbox6);
				if(wholeGameArray[6] == 0)
					highlightIndividualSectorRed (vbox7);
				if(wholeGameArray[7] == 0)
					highlightIndividualSectorRed (vbox8);
				if(wholeGameArray[8] == 0)
					highlightIndividualSectorRed (vbox9);
			} else if (sector == 6) {
				if(wholeGameArray[0] == 0)
					highlightIndividualSectorRed (vbox1);
				if(wholeGameArray[1] == 0)
					highlightIndividualSectorRed (vbox2);
				if(wholeGameArray[2] == 0)
					highlightIndividualSectorRed (vbox3);
				if(wholeGameArray[3] == 0)
					highlightIndividualSectorRed (vbox4);
				if(wholeGameArray[4] == 0)
					highlightIndividualSectorRed (vbox5);
				if(wholeGameArray[5] == 0)
					highlightIndividualSectorGreen (vbox6);
				if(wholeGameArray[6] == 0)
					highlightIndividualSectorRed (vbox7);
				if(wholeGameArray[7] == 0)
					highlightIndividualSectorRed (vbox8);
				if(wholeGameArray[8] == 0)
					highlightIndividualSectorRed (vbox9);
			} else if (sector == 7) {
				if(wholeGameArray[0] == 0)
					highlightIndividualSectorRed (vbox1);
				if(wholeGameArray[1] == 0)
					highlightIndividualSectorRed (vbox2);
				if(wholeGameArray[2] == 0)
					highlightIndividualSectorRed (vbox3);
				if(wholeGameArray[3] == 0)
					highlightIndividualSectorRed (vbox4);
				if(wholeGameArray[4] == 0)
					highlightIndividualSectorRed (vbox5);
				if(wholeGameArray[5] == 0)
					highlightIndividualSectorRed (vbox6);
				if(wholeGameArray[6] == 0)
					highlightIndividualSectorGreen (vbox7);
				if(wholeGameArray[7] == 0)
					highlightIndividualSectorRed (vbox8);
				if(wholeGameArray[8] == 0)
					highlightIndividualSectorRed (vbox9);
			} else if (sector == 8) {
				if(wholeGameArray[0] == 0)
					highlightIndividualSectorRed (vbox1);
				if(wholeGameArray[1] == 0)
					highlightIndividualSectorRed (vbox2);
				if(wholeGameArray[2] == 0)
					highlightIndividualSectorRed (vbox3);
				if(wholeGameArray[3] == 0)
					highlightIndividualSectorRed (vbox4);
				if(wholeGameArray[4] == 0)
					highlightIndividualSectorRed (vbox5);
				if(wholeGameArray[5] == 0)
					highlightIndividualSectorRed (vbox6);
				if(wholeGameArray[6] == 0)
					highlightIndividualSectorRed (vbox7);
				if(wholeGameArray[7] == 0)
					highlightIndividualSectorGreen (vbox8);
				if(wholeGameArray[8] == 0)
					highlightIndividualSectorRed (vbox9);
			} else if (sector == 9) {
				if(wholeGameArray[0] == 0)
					highlightIndividualSectorRed (vbox1);
				if(wholeGameArray[1] == 0)
					highlightIndividualSectorRed (vbox2);
				if(wholeGameArray[2] == 0)
					highlightIndividualSectorRed (vbox3);
				if(wholeGameArray[3] == 0)
					highlightIndividualSectorRed (vbox4);
				if(wholeGameArray[4] == 0)
					highlightIndividualSectorRed (vbox5);
				if(wholeGameArray[5] == 0)
					highlightIndividualSectorRed (vbox6);
				if(wholeGameArray[6] == 0)
					highlightIndividualSectorRed (vbox7);
				if(wholeGameArray[7] == 0)
					highlightIndividualSectorRed (vbox8);
				if(wholeGameArray[8] == 0)
					highlightIndividualSectorGreen (vbox9);
			}

		}


		static void buttonCallbackAssigner(Button button){

			if (buttonCreatedCounter == 1) {
				button.Clicked += button1Callback;
			}

			else if (buttonCreatedCounter == 2) {
				button.Clicked += button2Callback;
			}

			else if (buttonCreatedCounter == 3) {
				button.Clicked += button3Callback;
			}

			else if (buttonCreatedCounter == 4) {
				button.Clicked += button4Callback;
			}

			else if (buttonCreatedCounter == 5) {
				button.Clicked += button5Callback;
			}

			else if (buttonCreatedCounter == 6) {
				button.Clicked += button6Callback;
			}

			else if (buttonCreatedCounter == 7) {
				button.Clicked += button7Callback;
			}

			else if (buttonCreatedCounter == 8) {
				button.Clicked += button8Callback;
			}

			else if (buttonCreatedCounter == 9) {
				button.Clicked += button9Callback;
			}

			else if (buttonCreatedCounter == 10) {
				button.Clicked += button10Callback;
			}

			else if (buttonCreatedCounter == 11) {
				button.Clicked += button11Callback;
			}

			else if (buttonCreatedCounter == 12) {
				button.Clicked += button12Callback;
			}

			else if (buttonCreatedCounter == 13) {
				button.Clicked += button13Callback;
			}

			else if (buttonCreatedCounter == 14) {
				button.Clicked += button14Callback;
			}

			else if (buttonCreatedCounter == 15) {
				button.Clicked += button15Callback;
			}

			else if (buttonCreatedCounter == 16) {
				button.Clicked += button16Callback;
			}

			else if (buttonCreatedCounter == 17) {
				button.Clicked += button17Callback;
			}

			else if (buttonCreatedCounter == 18) {
				button.Clicked += button18Callback;
			}

			else if (buttonCreatedCounter == 19) {
				button.Clicked += button19Callback;
			}
			else if (buttonCreatedCounter == 20) {
				button.Clicked += button20Callback;
			}

			else if (buttonCreatedCounter == 21) {
				button.Clicked += button21Callback;
			}

			else if (buttonCreatedCounter == 22) {
				button.Clicked += button22Callback;
			}

			else if (buttonCreatedCounter == 23) {
				button.Clicked += button23Callback;
			}

			else if (buttonCreatedCounter == 24) {
				button.Clicked += button24Callback;
			}

			else if (buttonCreatedCounter == 25) {
				button.Clicked += button25Callback;
			}

			else if (buttonCreatedCounter == 26) {
				button.Clicked += button26Callback;
			}

			else if (buttonCreatedCounter == 27) {
				button.Clicked += button27Callback;
			}

			else if (buttonCreatedCounter == 28) {
				button.Clicked += button28Callback;
			}

			else if (buttonCreatedCounter == 29) {
				button.Clicked += button29Callback;
			}

			else if (buttonCreatedCounter == 30) {
				button.Clicked += button30Callback;
			}

			else if (buttonCreatedCounter == 31) {
				button.Clicked += button31Callback;
			}

			else if (buttonCreatedCounter == 32) {
				button.Clicked += button32Callback;
			}

			else if (buttonCreatedCounter == 33) {
				button.Clicked += button33Callback;
			}

			else if (buttonCreatedCounter == 34) {
				button.Clicked += button34Callback;
			}

			else if (buttonCreatedCounter == 35) {
				button.Clicked += button35Callback;
			}

			else if (buttonCreatedCounter == 36) {
				button.Clicked += button36Callback;
			}

			else if (buttonCreatedCounter == 37) {
				button.Clicked += button37Callback;
			}

			else if (buttonCreatedCounter == 38) {
				button.Clicked += button38Callback;
			}

			else if (buttonCreatedCounter == 39) {
				button.Clicked += button39Callback;
			}

			else if (buttonCreatedCounter == 40) {
				button.Clicked += button40Callback;
			}

			else if (buttonCreatedCounter == 41) {
				button.Clicked += button41Callback;
			}

			else if (buttonCreatedCounter == 42) {
				button.Clicked += button42Callback;
			}

			else if (buttonCreatedCounter == 43) {
				button.Clicked += button43Callback;
			}

			else if (buttonCreatedCounter == 44) {
				button.Clicked += button44Callback;
			}

			else if (buttonCreatedCounter == 45) {
				button.Clicked += button45Callback;
			}

			else if (buttonCreatedCounter == 46) {
				button.Clicked += button46Callback;
			}

			else if (buttonCreatedCounter == 47) {
				button.Clicked += button47Callback;
			}

			else if (buttonCreatedCounter == 48) {
				button.Clicked += button48Callback;
			}

			else if (buttonCreatedCounter == 49) {
				button.Clicked += button49Callback;
			}

			else if (buttonCreatedCounter == 50) {
				button.Clicked += button50Callback;
			}

			else if (buttonCreatedCounter == 51) {
				button.Clicked += button51Callback;
			}

			else if (buttonCreatedCounter == 52) {
				button.Clicked += button52Callback;
			}

			else if (buttonCreatedCounter == 53) {
				button.Clicked += button53Callback;
			}

			else if (buttonCreatedCounter == 54) {
				button.Clicked += button54Callback;
			}

			else if (buttonCreatedCounter == 55) {
				button.Clicked += button55Callback;
			}

			else if (buttonCreatedCounter == 56) {
				button.Clicked += button56Callback;
			}

			else if (buttonCreatedCounter == 57) {
				button.Clicked += button57Callback;
			}

			else if (buttonCreatedCounter == 58) {
				button.Clicked += button58Callback;
			}

			else if (buttonCreatedCounter == 59) {
				button.Clicked += button59Callback;
			}

			else if (buttonCreatedCounter == 60) {
				button.Clicked += button60Callback;
			}

			else if (buttonCreatedCounter == 61) {
				button.Clicked += button61Callback;
			}

			else if (buttonCreatedCounter == 62) {
				button.Clicked += button62Callback;
			}

			else if (buttonCreatedCounter == 63) {
				button.Clicked += button63Callback;
			}

			else if (buttonCreatedCounter == 64) {
				button.Clicked += button64Callback;
			}

			else if (buttonCreatedCounter == 65) {
				button.Clicked += button65Callback;
			}

			else if (buttonCreatedCounter == 66) {
				button.Clicked += button66Callback;
			}

			else if (buttonCreatedCounter == 67) {
				button.Clicked += button67Callback;
			}

			else if (buttonCreatedCounter == 68) {
				button.Clicked += button68Callback;
			}

			else if (buttonCreatedCounter == 69) {
				button.Clicked += button69Callback;
			}

			else if (buttonCreatedCounter == 70) {
				button.Clicked += button70Callback;
			}

			else if (buttonCreatedCounter == 71) {
				button.Clicked += button71Callback;
			}

			else if (buttonCreatedCounter == 72) {
				button.Clicked += button72Callback;
			}

			else if (buttonCreatedCounter == 73) {
				button.Clicked += button73Callback;
			}

			else if (buttonCreatedCounter == 74) {
				button.Clicked += button74Callback;
			}

			else if (buttonCreatedCounter == 75) {
				button.Clicked += button75Callback;
			}

			else if (buttonCreatedCounter == 76) {
				button.Clicked += button76Callback;
			}

			else if (buttonCreatedCounter == 77) {
				button.Clicked += button77Callback;
			}

			else if (buttonCreatedCounter == 78) {
				button.Clicked += button78Callback;
			}

			else if (buttonCreatedCounter == 79) {
				button.Clicked += button79Callback;
			}

			else if (buttonCreatedCounter == 80) {
				button.Clicked += button80Callback;
			}

			else if (buttonCreatedCounter == 81) {
				button.Clicked += button81Callback;
			}
		}


		//----------------------START OF CALLBACK METHODS FOR HANDLING BUTTON CLICKS------------------------------

		static void button1Callback(object obj, EventArgs args){
			int thisSector = 1;
			Button button = (Button)obj;
			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				Console.WriteLine (playerTurnCounter);
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					buttonsClicked [0] = 1;
					gameArray1 [0] = 1;
				} else {
					button.Label = "O";
					buttonsClicked [0] = 1;
					gameArray1 [0] = 2;
				}

				grader (thisSector);
				if (wholeGameArray [0] == 0 && checkIfSectorIsFull(1) == 0)
					sector = 1;
				else
					sector = 0;
				highlightSector(); 

			} 
			else {
				//do nothing for now. possibly flash button red or something
			}
				

//			if(playerTurnCounter%2 != 0)
//				computerMoveToCorrectSector ();

		}

		static void button2Callback(object obj, EventArgs args){
			int thisSector = 1;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				Console.WriteLine (playerTurnCounter);
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					buttonsClicked [1] = 1;
					gameArray1 [1] = 1;
				} else {
					button.Label = "O";
					buttonsClicked [1] = 1;
					gameArray1 [1] = 2;
				}

				if (wholeGameArray [1] == 0 && checkIfSectorIsFull(2) == 0)
					sector = 2;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button3Callback(object obj, EventArgs args){
			int thisSector = 1;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				Console.WriteLine (playerTurnCounter);
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					buttonsClicked [2] = 1;
					gameArray1 [2] = 1;
				} else {
					button.Label = "O";
					buttonsClicked [2] = 1;
					gameArray1 [2] = 2;
				}

				if (wholeGameArray [2] == 0 && checkIfSectorIsFull(3) == 0)
					sector = 3;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button4Callback(object obj, EventArgs args){
			int thisSector = 1;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				Console.WriteLine (playerTurnCounter);
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					buttonsClicked [3] = 1;
					gameArray1 [3] = 1;
				} else {
					button.Label = "O";
					buttonsClicked [3] = 1;
					gameArray1 [3] = 2;
				}

				if (wholeGameArray [3] == 0 && checkIfSectorIsFull(4) == 0)
					sector = 4;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button5Callback(object obj, EventArgs args){
			int thisSector = 1;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				Console.WriteLine (playerTurnCounter);
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					buttonsClicked [4] = 1;
					gameArray1 [4] = 1;
				} else {
					button.Label = "O";
					buttonsClicked [4] = 1;
					gameArray1 [4] = 2;
				}

				if (wholeGameArray [4] == 0 && checkIfSectorIsFull(5) == 0)
					sector = 5;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}  
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button6Callback(object obj, EventArgs args){
			int thisSector = 1;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				Console.WriteLine (playerTurnCounter);
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					buttonsClicked [5] = 1;
					gameArray1 [5] = 1;
				} else {
					button.Label = "O";
					buttonsClicked [5] = 1;
					gameArray1 [5] = 2;
				}

				if (wholeGameArray [5] == 0 && checkIfSectorIsFull(6) == 0)
					sector = 6;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}  
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button7Callback(object obj, EventArgs args){
			int thisSector = 1;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				Console.WriteLine (playerTurnCounter);
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					buttonsClicked [6] = 1;
					gameArray1 [6] = 1;
				} else {
					button.Label = "O";
					buttonsClicked [6] = 1;
					gameArray1 [6] = 2;
				}

				if (wholeGameArray [6] == 0 && checkIfSectorIsFull(7) == 0)
					sector = 7;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}  
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button8Callback(object obj, EventArgs args){
			int thisSector = 1;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				Console.WriteLine (playerTurnCounter);
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					buttonsClicked [7] = 1;
					gameArray1 [7] = 1;
				} else {
					button.Label = "O";
					buttonsClicked [7] = 1;
					gameArray1 [7] = 2;
				}

				if (wholeGameArray [7] == 0 && checkIfSectorIsFull(8) == 0)
					sector = 8;
				else
					sector = 0;
				highlightSector();
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button9Callback(object obj, EventArgs args){
			int thisSector = 1;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				Console.WriteLine (playerTurnCounter);
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					buttonsClicked [8] = 1;
					gameArray1 [8] = 1;
				} else {
					button.Label = "O";
					buttonsClicked [8] = 1;
					gameArray1 [8] = 2;
				}

				if (wholeGameArray [8] == 0 && checkIfSectorIsFull(9) == 0)
					sector = 9;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		//------------------END OF SECTOR 1----------------------------------------------------------------------------
			
		static void button10Callback(object obj, EventArgs args){
			int thisSector = 2;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray2 [0] = 1;
				} else {
					button.Label = "O";
					gameArray2 [0] = 2;
				}

				if (wholeGameArray [0] == 0 && checkIfSectorIsFull(1) == 0)
					sector = 1;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button11Callback(object obj, EventArgs args){
			int thisSector = 2;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray2 [1] = 1;
				} else {
					button.Label = "O";
					gameArray2 [1] = 2;
				}

				grader(thisSector);
				if (wholeGameArray [1] == 0 && checkIfSectorIsFull(2) == 0)
					sector = 2;
				else
					sector = 0;
				highlightSector(); 
			}  
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button12Callback(object obj, EventArgs args){
			int thisSector = 2;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray2 [2] = 1;
				} else {
					button.Label = "O";
					gameArray2 [2] = 2;
				}

				if (wholeGameArray [2] == 0 && checkIfSectorIsFull(3) == 0)
					sector = 3;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button13Callback(object obj, EventArgs args){
			int thisSector = 2;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray2 [3] = 1;
				} else {
					button.Label = "O";
					gameArray2 [3] = 2;
				}

				if (wholeGameArray [3] == 0 && checkIfSectorIsFull(4) == 0)
					sector = 4;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}  
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button14Callback(object obj, EventArgs args){
			int thisSector = 2;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray2 [4] = 1;
				} else {
					button.Label = "O";
					gameArray2 [4] = 2;
				}

				if (wholeGameArray [4] == 0 && checkIfSectorIsFull(5) == 0)
					sector = 5;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}  
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button15Callback(object obj, EventArgs args){
			int thisSector = 2;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray2 [5] = 1;
				} else {
					button.Label = "O";
					gameArray2 [5] = 2;
				}

				if (wholeGameArray [5] == 0 && checkIfSectorIsFull(6) == 0)
					sector = 6;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button16Callback(object obj, EventArgs args){
			int thisSector = 2;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray2 [6] = 1;
				} else {
					button.Label = "O";
					gameArray2 [6] = 2;
				}

				if (wholeGameArray [6] == 0 && checkIfSectorIsFull(7) == 0)
					sector = 7;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button17Callback(object obj, EventArgs args){
			int thisSector = 2;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray2 [7] = 1;
				} else {
					button.Label = "O";
					gameArray2 [7] = 2;
				}

				if (wholeGameArray [7] == 0 && checkIfSectorIsFull(8) == 0)
					sector = 8;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button18Callback(object obj, EventArgs args){
			int thisSector = 2;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray2 [8] = 1;
				} else {
					button.Label = "O";
					gameArray2 [8] = 2;
				}

				if (wholeGameArray [8] == 0 && checkIfSectorIsFull(9) == 0)
					sector = 9;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}  
			else {
				//do nothing for now. possibly flash button red or something
			}


		}


		//-----------------------END OF SECTOR 2---------------------------------------------------------


		static void button19Callback(object obj, EventArgs args){
			int thisSector = 3;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray3 [0] = 1;
				} else {
					button.Label = "O";
					gameArray3 [0] = 2;
				}

				if (wholeGameArray [0] == 0 && checkIfSectorIsFull(1) == 0)
					sector = 1;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button20Callback(object obj, EventArgs args){
			int thisSector = 3;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray3 [1] = 1;
				} else {
					button.Label = "O";
					gameArray3 [1] = 2;
				}
				if (wholeGameArray [1] == 0 && checkIfSectorIsFull(2) == 0)
					sector = 2;
				else
					sector = 0;
				highlightSector(); grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}
		static void button21Callback(object obj, EventArgs args){
			int thisSector = 3;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray3 [2] = 1;
				} else {
					button.Label = "O";
					gameArray3 [2] = 2;
				}

				grader(thisSector);
				if (wholeGameArray [2] == 0 && checkIfSectorIsFull(3) == 0)
					sector = 3;
				else
					sector = 0;
				highlightSector(); 
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button22Callback(object obj, EventArgs args){
			int thisSector = 3;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray3 [3] = 1;
				} else {
					button.Label = "O";
					gameArray3 [3] = 2;
				}

				if (wholeGameArray [3] == 0 && checkIfSectorIsFull(4) == 0)
					sector = 4;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button23Callback(object obj, EventArgs args){
			int thisSector = 3;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray3 [4] = 1;
				} else {
					button.Label = "O";
					gameArray3 [4] = 2;
				}

				if (wholeGameArray [4] == 0 && checkIfSectorIsFull(5) == 0)
					sector = 5;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}  
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button24Callback(object obj, EventArgs args){
			int thisSector = 3;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray3 [5] = 1;
				} else {
					button.Label = "O";
					gameArray3 [5] = 2;
				}

				if (wholeGameArray [5] == 0 && checkIfSectorIsFull(6) == 0)
					sector = 6;
				else
					sector = 0;
				highlightSector();
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button25Callback(object obj, EventArgs args){
			int thisSector = 3;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray3 [6] = 1;
				} else {
					button.Label = "O";
					gameArray3 [6] = 2;
				}

				if (wholeGameArray [6] == 0 && checkIfSectorIsFull(7) == 0)
					sector = 7;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}  
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button26Callback(object obj, EventArgs args){
			int thisSector = 3;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray3 [7] = 1;
				} else {
					button.Label = "O";
					gameArray3 [7] = 2;
				}

				if (wholeGameArray [7] == 0 && checkIfSectorIsFull(8) == 0)
					sector = 8;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button27Callback(object obj, EventArgs args){
			int thisSector = 3;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray3 [8] = 1;
				} else {
					button.Label = "O";
					gameArray3 [8] = 2;
				}

				if (wholeGameArray [8] == 0 && checkIfSectorIsFull(9) == 0)
					sector = 9;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}  
			else {
				//do nothing for now. possibly flash button red or something
			}


		}


		//------------------------------END OF SECTOR 3-------------------------------------------------------


		static void button28Callback(object obj, EventArgs args){
			int thisSector = 4;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray4 [0] = 1;
				} else {
					button.Label = "O";
					gameArray4 [0] = 2;
				}

				if (wholeGameArray [0] == 0 && checkIfSectorIsFull(1) == 0)
					sector = 1;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button29Callback(object obj, EventArgs args){
			int thisSector = 4;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray4 [1] = 1;
				} else {
					button.Label = "O";
					gameArray4 [1] = 2;
				}

				if (wholeGameArray [1] == 0 && checkIfSectorIsFull(2) == 0)
					sector = 2;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button30Callback(object obj, EventArgs args){
			int thisSector = 4;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray4 [2] = 1;
				} else {
					button.Label = "O";
					gameArray4 [2] = 2;
				}

				if (wholeGameArray [2] == 0 && checkIfSectorIsFull(3) == 0)
					sector = 3;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}
		static void button31Callback(object obj, EventArgs args){
			int thisSector = 4;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray4 [3] = 1;
				} else {
					button.Label = "O";
					gameArray4 [3] = 2;
				}

				grader(thisSector);
				if (wholeGameArray [3] == 0 && checkIfSectorIsFull(4) == 0)
					sector = 4;
				else
					sector = 0;
				highlightSector(); 

			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button32Callback(object obj, EventArgs args){
			int thisSector = 4;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray4 [4] = 1;
				} else {
					button.Label = "O";
					gameArray4 [4] = 2;
				}

				if (wholeGameArray [4] == 0 && checkIfSectorIsFull(5) == 0)
					sector = 5;
				else
					sector = 0;
				highlightSector();
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button33Callback(object obj, EventArgs args){
			int thisSector = 4;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray4 [5] = 1;
				} else {
					button.Label = "O";
					gameArray4 [5] = 2;
				}

				if (wholeGameArray [5] == 0 && checkIfSectorIsFull(6) == 0)
					sector = 6;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button34Callback(object obj, EventArgs args){
			int thisSector = 4;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray4 [6] = 1;
				} else {
					button.Label = "O";
					gameArray4 [6] = 2;
				}

				if (wholeGameArray [6] == 0 && checkIfSectorIsFull(7) == 0)
					sector = 7;
				else
					sector = 0;
				highlightSector();
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button35Callback(object obj, EventArgs args){
			int thisSector = 4;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray4 [7] = 1;
				} else {
					button.Label = "O";
					gameArray4 [7] = 2;
				}

				if (wholeGameArray [7] == 0 && checkIfSectorIsFull(8) == 0)
					sector = 8;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button36Callback(object obj, EventArgs args){
			int thisSector = 4;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray4 [8] = 1;
				} else {
					button.Label = "O";
					gameArray4 [8] = 2;
				}

				if (wholeGameArray [8] == 0 && checkIfSectorIsFull(9) == 0)
					sector = 9;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}


		//------------------------------END OF SECTOR 4-----------------------------------------------


		static void button37Callback(object obj, EventArgs args){
			int thisSector = 5;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray5 [0] = 1;
				} else {
					button.Label = "O";
					gameArray5 [0] = 2;
				}

				if (wholeGameArray [0] == 0 && checkIfSectorIsFull(1) == 0)
					sector = 1;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button38Callback(object obj, EventArgs args){
			int thisSector = 5;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray5 [1] = 1;
				} else {
					button.Label = "O";
					gameArray5 [1] = 2;
				}

				if (wholeGameArray [1] == 0 && checkIfSectorIsFull(2) == 0)
					sector = 2;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button39Callback(object obj, EventArgs args){
			int thisSector = 5;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray5 [2] = 1;
				} else {
					button.Label = "O";
					gameArray5 [2] = 2;
				}

				if (wholeGameArray [2] == 0 && checkIfSectorIsFull(3) == 0)
					sector = 3;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button40Callback(object obj, EventArgs args){
			int thisSector = 5;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray5 [3] = 1;
				} else {
					button.Label = "O";
					gameArray5 [3] = 2;
				}

				if (wholeGameArray [3] == 0 && checkIfSectorIsFull(4) == 0)
					sector = 4;
				else
					sector = 0;
				highlightSector();
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}
		static void button41Callback(object obj, EventArgs args){
			int thisSector = 5;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray5 [4] = 1;
				} else {
					button.Label = "O";
					gameArray5 [4] = 2;
				}

				grader(thisSector);
				if (wholeGameArray [4] == 0 && checkIfSectorIsFull(5) == 0)
					sector = 5;
				else
					sector = 0;
				highlightSector(); 

			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button42Callback(object obj, EventArgs args){
			int thisSector = 5;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray5 [5] = 1;
				} else {
					button.Label = "O";
					gameArray5 [5] = 2;
				}

				if (wholeGameArray [5] == 0 && checkIfSectorIsFull(6) == 0)
					sector = 6;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button43Callback(object obj, EventArgs args){
			int thisSector = 5;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray5 [6] = 1;
				} else {
					button.Label = "O";
					gameArray5 [6] = 2;
				}

				if (wholeGameArray [6] == 0 && checkIfSectorIsFull(7) == 0)
					sector = 7;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button44Callback(object obj, EventArgs args){
			int thisSector = 5;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray5 [7] = 1;
				} else {
					button.Label = "O";
					gameArray5 [7] = 2;
				}

				if (wholeGameArray [7] == 0 && checkIfSectorIsFull(8) == 0)
					sector = 8;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button45Callback(object obj, EventArgs args){
			int thisSector = 5;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray5 [8] = 1;
				} else {
					button.Label = "O";
					gameArray5 [8] = 2;
				}

				if (wholeGameArray [8] == 0 && checkIfSectorIsFull(9) == 0)
					sector = 9;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}


		//------------------------------END OF SECTOR 5----------------------------------------------------------


		static void button46Callback(object obj, EventArgs args){
			int thisSector = 6;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray6 [0] = 1;
				} else {
					button.Label = "O";
					gameArray6 [0] = 2;
				}

				if (wholeGameArray [0] == 0 && checkIfSectorIsFull(1) == 0)
					sector = 1;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button47Callback(object obj, EventArgs args){
			int thisSector = 6;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray6 [1] = 1;
				} else {
					button.Label = "O";
					gameArray6 [1] = 2;
				}

				if (wholeGameArray [1] == 0 && checkIfSectorIsFull(2) == 0)
					sector = 2;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button48Callback(object obj, EventArgs args){
			int thisSector = 6;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray6 [2] = 1;
				} else {
					button.Label = "O";
					gameArray6 [2] = 2;
				}

				if (wholeGameArray [2] == 0 && checkIfSectorIsFull(3) == 0)
					sector = 3;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button49Callback(object obj, EventArgs args){
			int thisSector = 6;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray6 [3] = 1;
				} else {
					button.Label = "O";
					gameArray6 [3] = 2;
				}

				if (wholeGameArray [3] == 0 && checkIfSectorIsFull(4) == 0)
					sector = 4;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button50Callback(object obj, EventArgs args){
			int thisSector = 6;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray6 [4] = 1;
				} else {
					button.Label = "O";
					gameArray6 [4] = 2;
				}

				if (wholeGameArray [4] == 0 && checkIfSectorIsFull(5) == 0)
					sector = 5;
				else
					sector = 0;
				highlightSector();
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}
		static void button51Callback(object obj, EventArgs args){
			int thisSector = 6;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray6 [5] = 1;
				} else {
					button.Label = "O";
					gameArray6 [5] = 2;
				}

				grader(thisSector);
				if (wholeGameArray [5] == 0 && checkIfSectorIsFull(6) == 0)
					sector = 6;
				else
					sector = 0;
				highlightSector(); 

			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button52Callback(object obj, EventArgs args){
			int thisSector = 6;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray6 [6] = 1;
				} else {
					button.Label = "O";
					gameArray6 [6] = 2;
				}

				if (wholeGameArray [6] == 0 && checkIfSectorIsFull(7) == 0)
					sector = 7;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button53Callback(object obj, EventArgs args){
			int thisSector = 6;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray6 [7] = 1;
				} else {
					button.Label = "O";
					gameArray6 [7] = 2;
				}

				if (wholeGameArray [7] == 0 && checkIfSectorIsFull(8) == 0)
					sector = 8;
				else
					sector = 0;
				highlightSector();
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button54Callback(object obj, EventArgs args){
			int thisSector = 6;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray6 [8] = 1;
				} else {
					button.Label = "O";
					gameArray6 [8] = 2;
				}

				if (wholeGameArray [8] == 0 && checkIfSectorIsFull(9) == 0)
					sector = 9;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}


		//------------------------------END OF SECTOR 6---------------------------------------------------------


		static void button55Callback(object obj, EventArgs args){
			int thisSector = 7;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray7 [0] = 1;
				} else {
					button.Label = "O";
					gameArray7 [0] = 2;
				}

				if (wholeGameArray [0] == 0 && checkIfSectorIsFull(1) == 0)
					sector = 1;
				else
					sector = 0;
				highlightSector();
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button56Callback(object obj, EventArgs args){
			int thisSector = 7;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray7 [1] = 1;
				} else {
					button.Label = "O";
					gameArray7 [1] = 2;
				}

				if (wholeGameArray [1] == 0 && checkIfSectorIsFull(2) == 0)
					sector = 2;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button57Callback(object obj, EventArgs args){
			int thisSector = 7;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray7 [2] = 1;
				} else {
					button.Label = "O";
					gameArray7 [2] = 2;
				}

				if (wholeGameArray [2] == 0 && checkIfSectorIsFull(3) == 0)
					sector = 3;
				else
					sector = 0;
				highlightSector();
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button58Callback(object obj, EventArgs args){
			int thisSector = 7;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray7 [3] = 1;
				} else {
					button.Label = "O";
					gameArray7 [3] = 2;
				}

				if (wholeGameArray [3] == 0 && checkIfSectorIsFull(4) == 0)
					sector = 4;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button59Callback(object obj, EventArgs args){
			int thisSector = 7;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray7 [4] = 1;
				} else {
					button.Label = "O";
					gameArray7 [4] = 2;
				}

				if (wholeGameArray [4] == 0 && checkIfSectorIsFull(5) == 0)
					sector = 5;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button60Callback(object obj, EventArgs args){
			int thisSector = 7;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray7 [5] = 1;
				} else {
					button.Label = "O";
					gameArray7 [5] = 2;
				}

				if (wholeGameArray [5] == 0 && checkIfSectorIsFull(6) == 0)
					sector = 6;
				else
					sector = 0;
				highlightSector();
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}
		static void button61Callback(object obj, EventArgs args){
			int thisSector = 7;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray7 [6] = 1;
				} else {
					button.Label = "O";
					gameArray7 [6] = 2;
				}

				grader(thisSector);
				if (wholeGameArray [6] == 0 && checkIfSectorIsFull(7) == 0)
					sector = 7;
				else
					sector = 0;
				highlightSector(); 

			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button62Callback(object obj, EventArgs args){
			int thisSector = 7;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray7 [7] = 1;
				} else {
					button.Label = "O";
					gameArray7 [7] = 2;
				}

				if (wholeGameArray [7] == 0 && checkIfSectorIsFull(8) == 0)
					sector = 8;
				else
					sector = 0;
				highlightSector();
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button63Callback(object obj, EventArgs args){
			int thisSector = 7;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray7 [8] = 1;
				} else {
					button.Label = "O";
					gameArray7 [8] = 2;
				}

				if (wholeGameArray [8] == 0 && checkIfSectorIsFull(9) == 0)
					sector = 9;
				else
					sector = 0;
				highlightSector();
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}


		//----------------------------------END OF SECTOR 7---------------------------------------------------------


		static void button64Callback(object obj, EventArgs args){
			int thisSector = 8;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray8 [0] = 1;
				} else {
					button.Label = "O";
					gameArray8 [0] = 2;
				}

				if (wholeGameArray [0] == 0 && checkIfSectorIsFull(1) == 0)
					sector = 1;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button65Callback(object obj, EventArgs args){
			int thisSector = 8;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray8 [1] = 1;
				} else {
					button.Label = "O";
					gameArray8 [1] = 2;
				}

				if (wholeGameArray [1] == 0 && checkIfSectorIsFull(2) == 0)
					sector = 2;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button66Callback(object obj, EventArgs args){
			int thisSector = 8;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray8 [2] = 1;
				} else {
					button.Label = "O";
					gameArray8 [2] = 2;
				}

				if (wholeGameArray [2] == 0 && checkIfSectorIsFull(3) == 0)
					sector = 3;
				else
					sector = 0;
				highlightSector();
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button67Callback(object obj, EventArgs args){
			int thisSector = 8;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray8 [3] = 1;
				} else {
					button.Label = "O";
					gameArray8 [3] = 2;
				}

				if (wholeGameArray [3] == 0 && checkIfSectorIsFull(4) == 0)
					sector = 4;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button68Callback(object obj, EventArgs args){
			int thisSector = 8;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray8 [4] = 1;
				} else {
					button.Label = "O";
					gameArray8 [4] = 2;
				}

				if (wholeGameArray [4] == 0 && checkIfSectorIsFull(5) == 0)
					sector = 5;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button69Callback(object obj, EventArgs args){
			int thisSector = 8;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray8 [5] = 1;
				} else {
					button.Label = "O";
					gameArray8 [5] = 2;
				}

				if (wholeGameArray [5] == 0 && checkIfSectorIsFull(6) == 0)
					sector = 6;
				else
					sector = 0;
				highlightSector();
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button70Callback(object obj, EventArgs args){
			int thisSector = 8;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray8 [6] = 1;
				} else {
					button.Label = "O";
					gameArray8 [6] = 2;
				}

				if (wholeGameArray [6] == 0 && checkIfSectorIsFull(7) == 0)
					sector = 7;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button71Callback(object obj, EventArgs args){
			int thisSector = 8;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray8 [7] = 1;
				} else {
					button.Label = "O";
					gameArray8 [7] = 2;
				}

				grader(thisSector);
				if (wholeGameArray [7] == 0 && checkIfSectorIsFull(8) == 0)
					sector = 8;
				else
					sector = 0;
				highlightSector(); 

			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button72Callback(object obj, EventArgs args){
			int thisSector = 8;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray8 [8] = 1;
				} else {
					button.Label = "O";
					gameArray8 [8] = 2;
				}

				if (wholeGameArray [8] == 0 && checkIfSectorIsFull(9) == 0)
					sector = 9;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}


		//----------------------------------END OF SECTOR 8-----------------------------------------------------------


		static void button73Callback(object obj, EventArgs args){
			int thisSector = 9;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray9 [0] = 1;
				} else {
					button.Label = "O";
					gameArray9 [0] = 2;
				}

				if (wholeGameArray [0] == 0 && checkIfSectorIsFull(1) == 0)
					sector = 1;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button74Callback(object obj, EventArgs args){
			int thisSector = 9;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray9 [1] = 1;
				} else {
					button.Label = "O";
					gameArray9 [1] = 2;
				}

				if (wholeGameArray [1] == 0 && checkIfSectorIsFull(2) == 0)
					sector = 2;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			}
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button75Callback(object obj, EventArgs args){
			int thisSector = 9;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray9 [2] = 1;
				} else {
					button.Label = "O";
					gameArray9 [2] = 2;
				}

				if (wholeGameArray [2] == 0 && checkIfSectorIsFull(3) == 0)
					sector = 3;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button76Callback(object obj, EventArgs args){
			int thisSector = 9;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray9 [3] = 1;
				} else {
					button.Label = "O";
					gameArray9 [3] = 2;
				}

				if (wholeGameArray [3] == 0 && checkIfSectorIsFull(4) == 0)
					sector = 4;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button77Callback(object obj, EventArgs args){
			int thisSector = 9;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray9 [4] = 1;
				} else {
					button.Label = "O";
					gameArray9 [4] = 2;
				}

				if (wholeGameArray [4] == 0 && checkIfSectorIsFull(5) == 0)
					sector = 5;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button78Callback(object obj, EventArgs args){
			int thisSector = 9;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray9 [5] = 1;
				} else {
					button.Label = "O";
					gameArray9 [5] = 2;
				}

				if (wholeGameArray [5] == 0 && checkIfSectorIsFull(6) == 0)
					sector = 6;
				else
					sector = 0;
				highlightSector();
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button79Callback(object obj, EventArgs args){
			int thisSector = 9;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray9 [6] = 1;
				} else {
					button.Label = "O";
					gameArray9 [6] = 2;
				}

				if (wholeGameArray [6] == 0 && checkIfSectorIsFull(7) == 0)
					sector = 7;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button80Callback(object obj, EventArgs args){
			int thisSector = 9;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray9 [7] = 1;
				} else {
					button.Label = "O";
					gameArray9 [7] = 2;
				}

				if (wholeGameArray [7] == 0 && checkIfSectorIsFull(8) == 0)
					sector = 8;
				else
					sector = 0;
				highlightSector(); 
				grader(thisSector);
			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		static void button81Callback(object obj, EventArgs args){
			int thisSector = 9;
			Button button = (Button)obj;

			if ((sector == thisSector || sector == 0) && (button.Label != "X") && (button.Label != "O")) {
				playerTurnCounter++;
				if (playerTurnCounter % 2 == 0) {
					button.Label = "X";
					gameArray9 [8] = 1;
				} else {
					button.Label = "O";
					gameArray9 [8] = 2;
				}

				grader(thisSector);
				if (wholeGameArray [8] == 0 && checkIfSectorIsFull(9) == 0)
					sector = 9;
				else
					sector = 0;
				highlightSector(); 

			} 
			else {
				//do nothing for now. possibly flash button red or something
			}


		}

		//--------------------------------END OF SECTOR 9---------------------------------------------------------

	

		//returns 1 if X wins the game
		//returns 2 if O wins the game
		//returns 0 if there is no winner for the game
		public static int gradeGame(int[] gameArray){
			if (gameArray [0] == 1 && gameArray [4] == 1 && gameArray [8] == 1)
				return 1;
			else if (gameArray [0] == 2 && gameArray [4] == 2 && gameArray [8] == 2)
				return 2;
			else if (gameArray [2] == 1 && gameArray [4] == 1 && gameArray [6] == 1)
				return 1;
			else if (gameArray [2] == 2 && gameArray [4] == 2 && gameArray [6] == 2)
				return 2;
			else if (gameArray [0] == 1 && gameArray [1] == 1 && gameArray [2] == 1)
				return 1;
			else if (gameArray [0] == 2 && gameArray [1] == 2 && gameArray [2] == 2)
				return 2;
			else if (gameArray [3] == 1 && gameArray [4] == 1 && gameArray [5] == 1)
				return 1;
			else if (gameArray [3] == 2 && gameArray [4] == 2 && gameArray [5] == 2)
				return 2;
			else if (gameArray [6] == 1 && gameArray [7] == 1 && gameArray [8] == 1)
				return 1;
			else if (gameArray [6] == 2 && gameArray [7] == 2 && gameArray [8] == 2)
				return 2;
			else if (gameArray [0] == 1 && gameArray [3] == 1 && gameArray [6] == 1)
				return 1;
			else if (gameArray [0] == 2 && gameArray [3] == 2 && gameArray [6] == 2)
				return 2;
			else if (gameArray [1] == 1 && gameArray [4] == 1 && gameArray [7] == 1)
				return 1;
			else if (gameArray [1] == 2 && gameArray [4] == 2 && gameArray [7] == 2)
				return 2;
			else if (gameArray [2] == 1 && gameArray [5] == 1 && gameArray [8] == 1)
				return 1;
			else if (gameArray [2] == 2 && gameArray [5] == 2 && gameArray [8] == 2)
				return 2;
			else
				return 0;
		}

		//Method used to select which game should be graded and updates wholeGameArray
		//with the correct value
		static void grader(int thisSector){
			if (thisSector == 1)
				wholeGameArray [0] = gradeGame (gameArray1);
			else if (thisSector == 2)
				wholeGameArray [1] = gradeGame (gameArray2);
			else if (thisSector == 3)
				wholeGameArray [2] = gradeGame (gameArray3);
			else if (thisSector == 4)
				wholeGameArray [3] = gradeGame (gameArray4);
			else if (thisSector == 5)
				wholeGameArray [4] = gradeGame (gameArray5);
			else if (thisSector == 6)
				wholeGameArray [5] = gradeGame (gameArray6);
			else if (thisSector == 7)
				wholeGameArray [6] = gradeGame (gameArray7);
			else if (thisSector == 8)
				wholeGameArray [7] = gradeGame (gameArray8);
			else if (thisSector == 9)
				wholeGameArray [8] = gradeGame (gameArray9);

			updateGameBoardWithGamesWon ();
		}

		static void updateGameBoardWithGamesWon(){

			Frame[] sectorArray = new Frame[9];
			sectorArray [0] = vbox1;
			sectorArray [1] = vbox2;
			sectorArray [2] = vbox3;
			sectorArray [3] = vbox4;
			sectorArray [4] = vbox5;
			sectorArray [5] = vbox6;
			sectorArray [6] = vbox7;
			sectorArray [7] = vbox8;
			sectorArray [8] = vbox9;

			int i;
			for (i = 0; i < 9; i++) {
				if (wholeGameArray [i] != 0) {
					if (sectorFlags [i] == 0) {
						VBox child = (VBox)sectorArray [i].Child;
						child.Destroy ();
						Label label = new Label ();
						if (wholeGameArray [i] == 1)
							label.Markup = "<span size='88000' color='white'>X</span>";
						else
							label.Markup = "<span size='88000' color='white'>O</span>";
						sectorArray [i].Add (label);
						label.Show ();
						sectorFlags [i] = 1;
						int won = gradeGame (wholeGameArray);
						if (won == 1) {
							Console.WriteLine ("Winner is X!!!!!");
							showWinner (won);
						} else if (won == 2) {
							Console.WriteLine ("Winner is O!!!!");
							showWinner (won);
						} else if (won == 0) {
							//do nothing
						}

					}
				}
			}
		}



		static void computerMoveToCorrectSector(){
			Random rnd = new Random ();
			int num = rnd.Next (1, 10);

			var enumerable = myTable.AllChildren;
			IEnumerator sequenceEnum = enumerable.GetEnumerator ();

			int counter = 1;
			while (sequenceEnum.MoveNext ()) {
				if (counter == sector || sector == 0) {
					Frame sectorFrame = (Frame)sequenceEnum.Current;
					computerMoveToCorrectButton (sectorFrame, num);
				}
				counter++;
			}
		}

		static void computerMoveToCorrectButton(Frame sectorFrame, int rand){

			VBox sectorVbox = (VBox)sectorFrame.Child;

			var enumerable = sectorVbox.AllChildren;
			IEnumerator sequenceEnum = enumerable.GetEnumerator ();

			int counter = 1;
			while (sequenceEnum.MoveNext ()) {
				HBox curHbox = (HBox) sequenceEnum.Current;
				var enum2 = curHbox.AllChildren;
				IEnumerator seqEnum = enum2.GetEnumerator ();

				while(seqEnum.MoveNext()){
					Frame curButtonFrame = (Frame)seqEnum.Current;
					Button curButton = (Button) curButtonFrame.Child;
					if (counter == rand) {
						if (buttonsClicked[((sector-1)*9)+counter] == 0) {
							curButton.Click ();
							return;
						} else {
							computerMoveToCorrectSector ();
							return;
						}
					}
					counter++;
				}
			}
		}


		//pass in 1 for X is winner and 2 for O is winner
		static void showWinner(int whoWon){
			if (whoWon == 1) {
				Table table = (Table)myWin.Child;
				table.Destroy ();
				Label label = new Label ();
				label.Markup = "<span size='100000' color='white'>You Are A Winner \n Player X !!!</span>";
				myWin.Add (label);
				label.Show ();
			} else if (whoWon == 2) {
				Table table = (Table)myWin.Child;
				table.Destroy ();
				Label label = new Label ();
				label.Markup = "<span size='100000' color='white'>You Are A Winner \n Player O !!!</span>";
				myWin.Add (label);
				label.Show ();
			}
		}


		public void Dispose(){

		}



			
	}




}
