import QtQuick 2.15
import QtQuick.Controls 2.15

ApplicationWindow {
  visible: true
  width: 640
  height: 480
  title: "Hello Qt Quick App"

  Item {
    anchors.fill: parent

    Rectangle {
      width: parent.width
      height: parent.height
      color: "white"

      Column {
        anchors.centerIn: parent

        Label {
          text: mainWindow.labelText
          font.pixelSize: 24
          color: "black"
        }

        TextField {
          width: 200
          text: mainWindow.textFieldText
          placeholderText: "Type something..."
        }

        Button {
          text: "Update Label"
          onClicked: mainWindow.updateLabel();
        }
      }
    }
  }
}
