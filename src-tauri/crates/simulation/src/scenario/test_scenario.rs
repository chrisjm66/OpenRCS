use crate::{layout::test_layout::make_test_layout, scenario::Scenario};

pub fn make_test_scenario() -> Scenario {
    Scenario { id: String::from("T1"), name: String::from("Test 1"), description: String::from("This is a description"), layout: make_test_layout() }
}
