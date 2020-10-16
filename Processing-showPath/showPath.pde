BufferedReader reader;
String line;

void setup() {
  size(800,600);
  PImage map = loadImage("minecraft.png");
  background(map);
  frameRate(1000000000);
  reader = createReader("test.txt");
  System.out.println("Started");
  loadPixels();  
}

void draw() {
  try {
    line = reader.readLine();
    if (line == null || line == "") {
      System.out.println("File read");
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
  int x = Integer.parseInt(split(coords[0], ",")[0]);
  int y = Integer.parseInt(split(coords[1], ",")[0]);
  int z = Integer.parseInt(split(coords[2], ",")[0]);
  color c = color(255,0,0);
  pixels[(600 - z)*800 + x*2] = c;
  pixels[(600 - z)*800 + x*2 + 1] = c;
  pixels[(600 - z)*800 + x*2 + 800 + 1] = c;
  pixels[(600 - z)*800 + x*2 + 800] = c;
  updatePixels();
}
