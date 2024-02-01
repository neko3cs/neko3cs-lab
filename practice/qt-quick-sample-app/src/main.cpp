#include <QGuiApplication>
#include <QQmlApplicationEngine>
#include <QQmlContext>

int main(int argc, char const *argv[])
{
  QCoreApplication::setAttribute(Qt::AA_EnableHighDpiScaling);
  QGuiApplication app(argc, argv);

  QQmlApplicationEngine engine;
  const QUrl url(QStringLiteral("qrc:/mainwindow.qml"));
  QObject::connect(
      &engine, &QQmlApplicationEngine::objectCreated, &app, [url](QObject *obj, const QUrl &objUrl)
      {
    if (!obj && url == objUrl) {
      QCoreApplication::exit(-1);
    } },
      Qt::QueuedConnection);
  QObject::connect(&engine, &QQmlApplicationEngine::quit, &QGuiApplication::quit);

  engine.load(url);
  
  return app.exec();
}
