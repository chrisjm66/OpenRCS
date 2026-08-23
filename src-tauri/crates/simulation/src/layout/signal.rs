use crate::layout::track::EdgeEnd;
use serde::{Deserialize, Serialize};

#[derive(Serialize, Deserialize)]
pub struct Signal {
    pub approach: EdgeEnd,
}

pub type SignalId = String;
