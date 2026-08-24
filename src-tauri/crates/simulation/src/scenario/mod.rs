use serde::{Deserialize, Serialize};
use specta::Type;
pub mod test_scenario;

use crate::layout::SimulationLayout;

#[derive(Serialize, Deserialize, Type)]
pub struct Scenario {
    id: String,
    name: String,
    description: String,
    layout: SimulationLayout
}