Feature: View product
@ignore
Scenario: Open product page - shows product name
    Given I have logged in as product manager
    And I have navigated to projects page
    When I press on product item
    Then I should be navigated to product page