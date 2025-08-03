@feature-tag
Feature: DemoApp feature

A short summary of the feature

@scenario-tag-1
Scenario: simple steps
    Given this is given step
    When this is when step
    Then this is then step

@scenario-tag-2
Scenario: steps with arguments
    Given given step with argument 'abcd'
    Then this is step with table:
    | book      | price |
    | sharpener | 30    |
    | pencil    | 15    |

@scenario-tag-1
Scenario: And keyword steps
    And this is the first step with And keyword
    When this is the second when step
        And this is the third when step