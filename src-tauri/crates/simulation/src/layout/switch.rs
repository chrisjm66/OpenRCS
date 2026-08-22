use crate::layout::track::EdgeEnd;

pub struct SwitchId(String);

pub struct Switch {
    common: EdgeEnd,
    normal: EdgeEnd,
    reverse: EdgeEnd,
}
