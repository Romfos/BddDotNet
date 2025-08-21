Feature: DemoApp feature

A short summary of the feature

Scenario: simple steps
    Given this is simple given step
    When this is simple when step
    Then this is simple then step

Scenario: simple async steps
    Given this is async task given step 
    Given this is async value task given step

Scenario: steps with arguments
    Given this is given step with argument 'abcd'
    Then this is then step with table:
    | book      | price |
    | sharpener | 30    |
    | pencil    | 15    |
    Then this is then step with doc string:
    """
    this is DocString text
    with multiple lines
    """

Scenario: And keyword steps
    And this is simple given step
    But this is simple given step
    When this is simple when step
        And this is simple when step