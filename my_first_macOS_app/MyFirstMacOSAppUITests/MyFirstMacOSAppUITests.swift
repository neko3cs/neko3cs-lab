//
//  MyFirstMacOSAppUITests.swift
//  MyFirstMacOSAppUITests
//
//  Created by neko3cs on 2026/02/14.
//

import XCTest

final class MyFirstMacOSAppUITests: XCTestCase {

    override func setUpWithError() throws {
        // Put setup code here. This method is called before the invocation of each test method in the class.

        // In UI tests it is usually best to stop immediately when a failure occurs.
        continueAfterFailure = false

        // In UI tests it’s important to set the initial state - such as interface orientation - required for your tests before they run. The setUp method is a good place to do this.
    }

    override func tearDownWithError() throws {
        // Put teardown code here. This method is called after the invocation of each test method in the class.
    }

    @MainActor
    func testCounterAndHistoryNavigation() throws {
        let app = XCUIApplication()
        app.launch()

        // 1. カウンター画面の確認
        // macOSではウィンドウタイトルや静的テキストとして現れることが多いため、汎用的な検索を行う
        XCTAssertTrue(app.staticTexts["カウンター"].waitForExistence(timeout: 5))
        
        // 2. カウントアップ操作
        let countUpButton = app.buttons["カウントアップ"]
        XCTAssertTrue(countUpButton.exists)
        countUpButton.click()
        
        // 3. サイドバーで履歴に切り替え
        // macOSのSidebarでは、NavigationLinkはボタンや行として認識されることが多い
        let sidebarHistoryLink = app.buttons["履歴"].exists ? app.buttons["履歴"] : app.staticTexts["履歴"]
        XCTAssertTrue(sidebarHistoryLink.waitForExistence(timeout: 5), "サイドバーの '履歴' が見つかりません")
        sidebarHistoryLink.click()
        
        // 4. 履歴画面の確認
        XCTAssertTrue(app.staticTexts["履歴"].waitForExistence(timeout: 5))
        
        // 履歴アイテム（テーブル/リスト/アウトライン）が存在するか確認
        // macOSのSwiftUI ListはOutlineとして認識されることが多い
        let historyList = app.outlines.firstMatch.exists ? app.outlines.firstMatch : app.tables.firstMatch
        XCTAssertTrue(historyList.waitForExistence(timeout: 5), "履歴リストが見つかりません。")
        
        // 5. SidebarでHttpCatに切り替え
        let sidebarHttpCatLink = app.buttons["HttpCat"].exists ? app.buttons["HttpCat"] : app.staticTexts["HttpCat"]
        XCTAssertTrue(sidebarHttpCatLink.waitForExistence(timeout: 5))
        sidebarHttpCatLink.click()
        
        // 6. HttpCat画面の操作
        XCTAssertTrue(app.staticTexts["HttpCat"].waitForExistence(timeout: 5))
        
        let randomButton = app.buttons["ランダムな猫を表示"]
        XCTAssertTrue(randomButton.exists)
        randomButton.click()
        
        // 画像（AsyncImage）が表示されるのを待機
        XCTAssertTrue(app.images.firstMatch.waitForExistence(timeout: 10))
    }


    @MainActor
    func testLaunchPerformance() throws {
        // This measures how long it takes to launch your application.
        measure(metrics: [XCTApplicationLaunchMetric()]) {
            XCUIApplication().launch()
        }
    }
}
