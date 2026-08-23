use crate::layout::track::EdgeEnd;
use serde::{Deserialize, Serialize};

pub type SwitchId = String;

#[derive(Serialize, Deserialize)]
pub struct Switch {
    pub common: EdgeEnd,
    pub normal: EdgeEnd,
    pub reverse: EdgeEnd,
}
