// Learn more about Tauri commands at https://tauri.app/develop/calling-rust/
use tauri_specta::{collect_commands, Builder};
#[tauri::command]
#[specta::specta]
fn get_layouts() -> Vec<simulation::scenario::Scenario> {
    vec![simulation::scenario::test_scenario::make_test_scenario()]
}

#[cfg_attr(mobile, tauri::mobile_entry_point)]
pub fn run() {
    let builder = Builder::<tauri::Wry>::new()
        .commands(collect_commands![
           get_layouts 
        ]);

    #[cfg(debug_assertions)]
    builder
        .export(
            specta_typescript::Typescript::default(),
            "../src/bindings.ts",
        )
        .expect("failed to export typescript bindings");

    tauri::Builder::default()
        .invoke_handler(builder.invoke_handler())
        .setup(move |app| {
            builder.mount_events(app);
            Ok(())
        })
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}
