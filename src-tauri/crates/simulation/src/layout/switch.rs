use crate::layout::track::EdgeEnd;
use serde::{Deserialize, Serialize};
use specta::Type;

pub type SwitchId = String;

#[derive(Serialize, Deserialize, Type)]
pub struct Switch {
    pub common: EdgeEnd,
    pub normal: EdgeEnd,
    pub reverse: EdgeEnd,
}
