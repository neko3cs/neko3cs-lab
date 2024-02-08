import QtQuick
import QtQuick.Controls

ApplicationWindow {
  visible: true
  width: 640
  height: 480
  title: "Hello Qt Quick App"
  background: Rectangle {
    color: "white"
  }

  Column {
    anchors.centerIn: parent
    spacing: 10

    Label {
      text: "Hello Qt Quick World!"
      font.pixelSize: 24
      color: "black"
      anchors.horizontalCenter: parent.horizontalCenter
    }

    TextField {
      id: textField
      placeholderText: "何か入力してね！"
      width: parent.width
      anchors.horizontalCenter: parent.horizontalCenter
    }

    Label {
      text: mainWindow.text
      font.pixelSize: 16
      color: "black"
      anchors.horizontalCenter: parent.horizontalCenter
    }

    Button {
      text: "表示を切り替える"
      onClicked: mainWindow.updateLabel(textField.text)  // HACK: idを使わないで更新する方法を探す
      anchors.horizontalCenter: parent.horizontalCenter
    }
  }
}
