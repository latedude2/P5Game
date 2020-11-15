BufferedReader reader;
String line;
color c;
boolean forwardTimeSaved = false;
boolean backTimeSaved = false;

float forwardTime;
float backTime;
int mistakes;
int shortcuts;

int sizeX = 1200;
int sizeY = 900;
int startBiasX = -140;
int startBiasZ = -50;
float stretchMultiplierX = 2.2;
float stretchMultiplierY = 2.2;

color forwardColor = color(0,0,255);
color backColor = color(255,0,0);
int drawAlpha;

//modify wanted output here
boolean drawForward = true;
boolean drawBack = true;
boolean drawAll = true;
boolean heatmap = true;

int heatmapCircleDiameter = 5;

String[] arrowFilenames;
String[] beeFilenames;

void setup() {
  size(1200,900);
  PImage map = loadImage("isometric.png");
  //map.filter(GRAY);
  background(map);
  frameRate(1000000000);
  loadPixels();  
  c = forwardColor;
  
  if(drawAll)
  {
    drawAlpha = 30;
    println("Files for arrow condition: ");
    arrowFilenames = listFileNames("C:/UnityProjects/P5Game/ProcessingShowPath/arrow");
    printArray(arrowFilenames);
  
    println("Files for bee condition: ");
    beeFilenames = listFileNames("C:/UnityProjects/P5Game/ProcessingShowPath/bee");
    printArray(beeFilenames);
  
    System.out.println("Drawing arrow condition files");
    System.out.println("participant_number, time_going_forward, time_going_back, mistakes_made, shortcuts_taken");
    for(int i = 0; i < arrowFilenames.length; i++)
    {
      reader = createReader("arrow/" + arrowFilenames[i]);
      drawPath(arrowFilenames[i]);
      forwardTimeSaved = false;
      backTimeSaved = false;
    }
    
    System.out.println("Drawing bee condition files");
    System.out.println("participant_number, time_going_forward, time_going_back, mistakes_made, shortcuts_taken");
    for(int i = 0; i < beeFilenames.length; i++)
    {
      reader = createReader("bee/" + beeFilenames[i]);
      drawPath(beeFilenames[i]);
      forwardTimeSaved = false;
      backTimeSaved = false;
    }
  }
  else 
   {
     drawAlpha = 100;
     reader = createReader("arrow/test.txt");
   }
}

void draw() {
  if(!drawAll)
  {
     drawSinglePathSlow();
  }
}

void drawSinglePathSlow()
{
  try {
    line = reader.readLine();
    if (line == null || line == "") {
      System.out.println("File read");
      System.out.println(forwardTime + ", " + backTime + ", " + mistakes + ", " + shortcuts);
      stop();
      noLoop();
    } else {
      
      lineDraw(line);
    }
  } catch (IOException e) {
    e.printStackTrace();
    stop();
    noLoop();
  }
}

void drawPath(String filename)
{
  try {
    while((line = reader.readLine())!= null)
    {
      lineDraw(line);
    }
    System.out.println(split(filename, ".")[0] + ", " + forwardTime + ", " + backTime + ", " + mistakes + ", " + shortcuts);
  }
  catch (IOException e)
  {
    e.printStackTrace();
  }
}


void lineDraw(String line){
  String[] coords = split(line, " ");
  if(!coords[0].equals("EndTaskCompleted"))  //If this is not the end task tag
  {
    if(coords.length > 2)    //if coordinates, not mistake and shortcut count
    {
      if(c == forwardColor && drawForward || c == backColor && drawBack)
      {
        coords[0] = coords[0].replace(',', '.');      //Processing only likes floats with "." and not ","
        coords[2] = coords[2].replace(',', '.');      //Processing only likes floats with "." and not ","
        int x = int(startBiasX + (stretchMultiplierX * Float.parseFloat(coords[0])));
        //int y = Integer.parseInt(split(coords[1], ",")[0]);
        int z = int(startBiasZ + (stretchMultiplierY * Float.parseFloat(coords[2])));
        
        
        if(heatmap)
        {
          stroke(c, drawAlpha / 10);
          fill(c, drawAlpha / 10);
          circle(x, sizeY - z, heatmapCircleDiameter);
        }
        else 
        {
          stroke(c, drawAlpha);
          point(x, sizeY - z);
          point(x + 1, sizeY - z);
          point(x + 1, sizeY - z + 1);
          point(x + 1, sizeY - z + 1);
        }
        
        if(!forwardTimeSaved)
        {
           coords[7] = coords[7].replace(',', '.');    //Processing only likes floats with "." and not ","
           forwardTime = Float.parseFloat(coords[7]);
        }
        if(!backTimeSaved)
        {
           coords[7] = coords[7].replace(',', '.');    //Processing only likes floats with "." and not ","
           backTime = Float.parseFloat(coords[7]) - forwardTime;
        }
      }
    }
    else //mistake and shortcut counts, last line in file
    {
        backTimeSaved = true;
        mistakes = Integer.parseInt(coords[0]);
        shortcuts = Integer.parseInt(coords[1]);
    }
  }
  else //This is the end task tag
  {
    forwardTimeSaved = true;
    c = backColor;
  }
}

// This function returns all the files in a directory as an array of Strings  
String[] listFileNames(String dir) {
  File file = new File(dir);
  if (file.isDirectory()) {
    String names[] = file.list();
    return names;
  } else {
    // If it's not a directory
    return null;
  }
}
