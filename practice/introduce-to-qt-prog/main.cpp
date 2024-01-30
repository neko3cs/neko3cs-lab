  #include <QApplication>
  #include <QLabel>
  // #include <QTextCodec>
  // #include <QString>

  int main(int argc, char** argv)
  {
    QApplication app(argc, argv);
    // QTextCodec::setCodecForTr(QTextCodec::codecForLocale());  // Qt6では文字コード対応しなくても日本語表示可能っぽい
    QLabel* label = new QLabel(QObject::tr("こんにちは Qt"));
    label->show();
    return app.exec();
  }