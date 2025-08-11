@feature-tag
Feature: DemoApp feature

A short summary of the feature

@scenario-tag-1
Scenario: simple steps
    Given this is simple given step
    When this is simple when step
    Then this is simple then step

@scenario-tag-1
Scenario: simple async steps
    Given this is async task given step 
    Given this is async value task given step

@scenario-tag-2
Scenario: steps with arguments
    Given this is given step with argument 'abcd'
    Then this is then step with table:
    | book      | price |
    | sharpener | 30    |
    | pencil    | 15    |

@scenario-tag-3
Scenario: And keyword steps
    And this is simple given step
    When this is simple when step
        And this is simple when step

@scenario-tag-4
Scenario Outline: scenario outline with 2 examples
    Given this is given step with argument '<book>'
    Then this is then step with table:
    | book   | price   |
    | <book> | <price> |
    | static | 99      |

    Examples: 
    | book      | price |
    | sharpener | 30    |

    Examples: 
    | book   | price |
    | pencil | 15    |

@scenario-tag-5
Scenario: step transformation scenario
    Given this is given step with table transformation:
    | Key    | Value |
    | first  | 1     |
    | second | 2     |

@scenario-tag-6
Scenario: step with model transformation scenario
    When this is given step with model transformation:
    | Name   | Value |
    | first  | 1     |
    | second | abcd  |