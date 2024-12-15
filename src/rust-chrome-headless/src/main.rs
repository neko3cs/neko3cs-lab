use std::error::Error;

use headless_chrome::protocol::cdp::Page;
use headless_chrome::Browser;

fn browse_wikipedia() -> Result<(), Box<dyn Error>> {
    let browser = Browser::default()?;
    let tab = browser.new_tab()?;

    tab.navigate_to("https://www.wikipedia.org")?;
    tab.wait_for_element("input#searchInput")?.click()?;
    tab.type_str("WebKit")?.press_key("Enter")?;

    let elem = tab.wait_for_element("#firstHeading")?;
    assert!(tab.get_url().ends_with("WebKit"));

    // スクショ取得
    let _jpeg_data =
        tab.capture_screenshot(Page::CaptureScreenshotFormatOption::Jpeg, None, None, true)?;
    let _png_data = tab
        .wait_for_element("#mw-content-text > div > table.infobox.vevent")?
        .capture_screenshot(Page::CaptureScreenshotFormatOption::Png)?;

    // JSコードを埋め込んで実行->結果取得
    let remote_object = elem.call_js_fn(
        r#"
        function getIdTwice () {
            // `this` is always the element that you called `call_js_fn` on
            const id = this.id;
            return id + id;
        }
    "#,
        vec![],
        false,
    )?;
    match remote_object.value {
        Some(returned_string) => {
            dbg!(&returned_string);
            assert_eq!(returned_string, "firstHeadingfirstHeading".to_string());
        }
        _ => unreachable!(),
    };

    Ok(())
}

fn main() {
    match browse_wikipedia() {
        Ok(it) => it,
        Err(err) => eprintln!("エラーが発生しました: {}", err),
    };
}
