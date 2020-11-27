BufferedReader reader;
String line;
color c;
boolean forwardTimeSaved = false;
boolean backTimeSaved = false;
float invalidTimeBecauseAidWasAbandoned = -1.0;

float forwardTime;
float backTime;
float wanderTimeSum; 
float frameTime = 0;

FloatList returnToBeeTimes = new FloatList();
FloatList wanderFromBeeTimes  = new FloatList();

int mistakes;
int shortcuts;

//Variables for matching path to map image
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
boolean drawAll = false;
boolean heatmap = false;
String singleFilePath = "arrow/Lina.txt";

int heatmapCircleDiameter = 5;

String[] arrowFilenames;
String[] beeFilenames;

void setup() {
  size(1200,900);
  PImage map = loadImage("isometric.png");
  map.filter(GRAY);
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
  
    println("Drawing arrow condition files");
    println("participant_number, time_going_forward, time_going_back, mistakes_made, shortcuts_taken, wander_count, wander_time_sum, finished");
    for(int i = 0; i < arrowFilenames.length; i++)
    {
      reader = createReader("arrow/" + arrowFilenames[i]);
      drawPath();
      FloatList wanderTimes = calculateTimesAwayFromBee(returnToBeeTimes, wanderFromBeeTimes);
      wanderTimeSum = sumOfTimes(wanderTimes);
      //printTimes(wanderTimes);
      printParticipantResult(arrowFilenames[i], wanderTimes);
      resetVariablesForNextTestRun();
    }
    
    println("Drawing bee condition files");
    println("participant_number, time_going_forward, time_going_back, mistakes_made, shortcuts_taken, wander_count, wander_time_sum, finished");
    for(int i = 0; i < beeFilenames.length; i++)
    {
      reader = createReader("bee/" + beeFilenames[i]);
      drawPath();
      FloatList wanderTimes = calculateTimesAwayFromBee(returnToBeeTimes, wanderFromBeeTimes);
      wanderTimeSum = sumOfTimes(wanderTimes);
      //printTimes(wanderTimes);
      printParticipantResult(beeFilenames[i], wanderTimes);
      resetVariablesForNextTestRun();
    }
  }
  else 
   {
     drawAlpha = 100;
     reader = createReader(singleFilePath);
   }
}

void draw() {
  if(!drawAll)
  {
     drawSinglePathSlow();
  }
}

void printParticipantResult(String filename, FloatList wanderTimes)
{
  String finishedString;
  
  if (wanderTimes.size() != 0 && wanderTimes.get(0) == invalidTimeBecauseAidWasAbandoned)
  {
      finishedString = "Left aid";
  }
  else if(backTimeSaved)
  {
    finishedString = "Yes";
  }
  else 
  {
    finishedString = "No";
  }
  println(split(filename, ".")[0] + ", " + forwardTime + ", " + backTime + ", " + mistakes + ", " + shortcuts + ", " + wanderTimes.size() + ", " + wanderTimeSum  + ", " + finishedString);
}

void drawSinglePathSlow()
{
  try {
    line = reader.readLine();
    if (line == null || line == "") {
      FloatList wanderTimes = calculateTimesAwayFromBee(returnToBeeTimes, wanderFromBeeTimes);
      wanderTimeSum = sumOfTimes(wanderTimes);
      println("participant_number, time_going_forward, time_going_back, mistakes_made, shortcuts_taken, wander_count, wander_time_sum, finished");
      printParticipantResult(singleFilePath, wanderTimes);
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
  }
  catch (IOException e)
  {
    e.printStackTrace();
  }
}


void lineDraw(String line){
  String[] coords = split(line, " ");
  if(coords.length > 2)    //if coordinates, not mistake and shortcut count
  {
    coords[0] = coords[0].replace(',', '.');      //Processing only likes floats with "." and not ","
    coords[2] = coords[2].replace(',', '.');      //Processing only likes floats with "." and not ","
    int x = int(startBiasX + (stretchMultiplierX * Float.parseFloat(coords[0])));
    //int y = Integer.parseInt(split(coords[1], ",")[0]);
    int z = int(startBiasZ + (stretchMultiplierY * Float.parseFloat(coords[2])));
      
    if(c == forwardColor && drawForward || c == backColor && drawBack)
    {
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
    }
    coords[7] = coords[7].replace(',', '.');  //Processing only likes floats with "." and not ","
    frameTime = Float.parseFloat(coords[7]);
    //println(forwardTimeSaved);
    if(!forwardTimeSaved)
    {
       forwardTime = frameTime;
    }
    if(!backTimeSaved)
    {
       backTime = frameTime - forwardTime;
    }
  }
  else if(coords.length > 1) //mistake and shortcut counts, last line in file
  {
    {
        mistakes = Integer.parseInt(coords[0]);
        shortcuts = Integer.parseInt(coords[1]);
    }
  }
  else if(line.equals("EndTaskCompleted")) //This is the end task tag
  {
    forwardTimeSaved = true;
    c = backColor;
  }
  else if(line.equals("ReturnedToBee")) 
  {
    returnToBeeTimes.append(frameTime);
  }
  else if(line.equals("WanderedFromBee")) 
  {
    wanderFromBeeTimes.append(frameTime);
  }
  else if(line.equals("TestCompleted"))
  {
    backTimeSaved = true;
  }
}

void printTimes(FloatList times)
{
  for(int i = 0; i < times.size(); i++)
  {
    println(times.get(i));
  }
}

float sumOfTimes(FloatList times)
{
  float sum = 0;
  for(int i = 0; i < times.size(); i++)
  {
    sum += times.get(i);
  }
  return sum;
}

FloatList calculateTimesAwayFromBee(FloatList returnTimes, FloatList awayTimes)
{
  FloatList times = new FloatList();
  if(returnTimes.size() == awayTimes.size())
  {
      times.append(invalidTimeBecauseAidWasAbandoned);
      return times;
  }
  for(int i = 0; i < awayTimes.size(); i++)
  {
    times.append(returnTimes.get(i + 1) - awayTimes.get(i));
  }
  return times;
}

void resetVariablesForNextTestRun()
{
  forwardTime = 0;
  backTime = 0;
  forwardTimeSaved = false;
  backTimeSaved = false;
  returnToBeeTimes = new FloatList();
  wanderFromBeeTimes  = new FloatList();
  frameTime = 0;
  c = forwardColor;
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
