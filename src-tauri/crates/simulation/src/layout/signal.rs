use crate::layout::track::EdgeEnd;
use serde::{Deserialize, Serialize};
use specta::Type;

#[derive(Serialize, Deserialize, Type)]
pub struct Signal {
    pub approach: EdgeEnd,
}

pub type SignalId = String;
