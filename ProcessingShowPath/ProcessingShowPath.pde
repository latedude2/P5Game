BufferedReader reader;
String line;
color c;

boolean forwardTimeSaved = false;
boolean backTimeSaved = false;

float forwardTime;
float backTime;
int mistakes;
int shortcuts;
int startBiasX = -140;
int startBiasZ = -50;
int sizeX = 1200;
int sizeY = 900;
float stretchMultiplierX = 2.2;
float stretchMultiplierY = 2.2;

String[] filenames1;
String[] filenames2;

color forwardColor = color(0,0,255);
color backColor = color(255,0,0);

boolean drawForward = true;
boolean drawBack = true;

void setup() {
  size(1200,900);
  PImage map = loadImage("isometric.png");
  //map.filter(GRAY);
  background(map);
  frameRate(1000000000);
  loadPixels();  
  c = forwardColor;
  println("Files for first condition: ");
  filenames1 = listFileNames("C:/UnityProjects/P5Game/ProcessingShowPath/Condition1");
  printArray(filenames1);

  println("Files for second condition: ");
  filenames2 = listFileNames("C:/UnityProjects/P5Game/ProcessingShowPath/Condition2");
  printArray(filenames2);
  
  
  System.out.println("Drawing first condition files");
  for(int i = 0; i < filenames1.length; i++)
  {
    reader = createReader("Condition1/" + filenames1[i]);
    drawPath();
  }
  
  System.out.println("Drawing second condition files");
  for(int i = 0; i < filenames2.length; i++)
  {
    reader = createReader("Condition2/" + filenames2[i]);
    drawPath();
  }
}

void draw() {
  //drawSinglePathSlow();
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

void drawPath()
{
  try {
    while((line = reader.readLine())!= null)
    {
      lineDraw(line);
    }
    System.out.println(forwardTime + ", " + backTime + ", " + mistakes + ", " + shortcuts);
    System.out.println("File read");
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
        
        stroke(c, 30);
        point(x, sizeY - z);
        point(x + 1, sizeY - z);
        point(x + 1, sizeY - z + 1);
        point(x + 1, sizeY - z + 1);
        
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

// This function returns all the files in a directory as an array of File objects
File[] listFiles(String dir) {
  File file = new File(dir);
  if (file.isDirectory()) {
    File[] files = file.listFiles();
    return files;
  } else {
    // If it's not a directory
    return null;
  }
}
