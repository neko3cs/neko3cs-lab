#include <GLUT/glut.h>

void display()
{
  glClear(GL_COLOR_BUFFER_BIT);

  glColor3d(1.0, 1.0, 0.0);
  glBegin(GL_LINE_LOOP);
  glVertex2d(-0.9, -0.9);
  glVertex2d(0.9, -0.9);
  glVertex2d(0.9, 0.9);
  glVertex2d(-0.9, 0.9);
  glEnd();

  glFlush();
}

void initGL()
{
  glutInitDisplayMode(GLUT_RGBA);
  glutInitWindowSize(640, 480);
  glutCreateWindow("Rectangle");
  glutDisplayFunc(display);
  glClearColor(0.0, 0.0, 1.0, 1.0);
}

int main(int argc, char *argv[])
{
  glutInit(&argc, argv);
  initGL();
  glutMainLoop();
  return 0;
}
