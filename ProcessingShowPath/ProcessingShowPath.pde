BufferedReader reader;
String line;
color c;

boolean forwardTimeSaved = false;
boolean backTimeSaved = false;

float forwardTime;
float backTime;
int mistakes;
int shortcuts;
int startBiasX = -120;
int startBiasZ = -300;

void setup() {
  size(1200,900);
  PImage map = loadImage("minecraft.png");
  background(map);
  frameRate(1000000000);
  reader = createReader("test.txt");
  System.out.println("Started");
  loadPixels();  
  c = color(0,0,255);
}

void draw() {
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
void lineDraw(String line){
  String[] coords = split(line, " ");
  if(!coords[0].equals("EndTaskCompleted"))  //If this is not the end task tag
  {
    if(coords.length > 2)    //if coordinates, not mistake and shortcut count
    {
      int x = int(startBiasX + (2.1 * Integer.parseInt(split(coords[0], ",")[0])));
      int y = Integer.parseInt(split(coords[1], ",")[0]);
      int z = int(startBiasZ + (2 * Integer.parseInt(split(coords[2], ",")[0])));
      
      pixels[(600 - z) * 1200 + x] = c;
      //pixels[(600 - z)*800 + x*2 + 1] = c;
      //pixels[(600 - z)*800 + x*2 + 800 + 1] = c;
      //pixels[(600 - z)*800 + x*2 + 800] = c;
      updatePixels();
      
      if(!forwardTimeSaved)
      {
         coords[7] = coords[7].replace(',', '.');
         forwardTime = Float.parseFloat(coords[7]);
      }
      if(!backTimeSaved)
      {
         coords[7] = coords[7].replace(',', '.');
         backTime = Float.parseFloat(coords[7]) - forwardTime;
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
    c = color(255,0,0);
  }
}
