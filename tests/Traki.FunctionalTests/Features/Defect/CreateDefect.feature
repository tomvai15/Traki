 
Feature: Create defect

Scenario: Create defect without without uploading image
    Given I have logged in as product manager
    And I have navigated to projects page
    And I have opened product page
    And I have opened defects page
    When I select new defect tab
    And I select region
    And fill defect information
    Then defects information is displayed