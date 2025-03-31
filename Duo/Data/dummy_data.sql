-- Insert Users
INSERT INTO Users (Username) VALUES 
('geo_learner1'), 
('map_master'), 
('continent_king');

-- Insert Roadmaps
INSERT INTO Roadmaps (Name) VALUES 
('World Geography');

-- Insert Sections
INSERT INTO Sections (SubjectId, Title, Description, RoadmapId, OrderNumber) VALUES 
(1, 'Introduction to Continents', 'Learn the basics of continents', 1, 1),
(1, 'Countries of Europe', 'Explore the countries in Europe', 1, 2),
(1, 'Asian Capitals', 'Memorize the capitals of Asia', 1, 3);

-- Insert Quizzes
INSERT INTO Quizzes (SectionId, OrderNumber) VALUES 
(1, 1),
(2, 1),
(1, 2);

-- Insert Exercises
INSERT INTO Exercises (Type, DifficultyId, Question) VALUES 
('MultipleChoice', 1, 'What is the largest continent?'),
('FillInTheBlank', 2, 'The capital of France is Paris.'),
('Association', 3, 'Match the countries with their capitals.'),
('Flashcard', 1, 'What is the capital of Japan?');

-- Retrieve Exercise IDs
DECLARE @mcExerciseId INT = 1;
DECLARE @fibExerciseId INT = 2;
DECLARE @assocExerciseId INT = 3;
DECLARE @flashcardExerciseId INT = 4;

-- Insert Multiple Choice Exercise Details
INSERT INTO MultipleChoiceExercises (ExerciseId, CorrectAnswer) VALUES 
(1, 'Asia');

INSERT INTO MultipleChoiceOptions (ExerciseId, OptionText) VALUES 
(1, 'Africa'),
(1, 'Europe'),
(1, 'North America');

-- Insert Fill In The Blank Answers
INSERT INTO FillInTheBlankExercises (ExerciseId) VALUES (2);

INSERT INTO FillInTheBlankAnswers (ExerciseId, CorrectAnswer) VALUES 
(2, 'Paris');

-- Insert Association Exercise
INSERT INTO AssociationExercises (ExerciseId) VALUES (3);

INSERT INTO AssociationPairs (ExerciseId, FirstAnswer, SecondAnswer) VALUES 
(3, 'France', 'Paris'),
(3, 'Germany', 'Berlin'),
(3, 'Italy', 'Rome');

-- Insert Flashcard Exercise
INSERT INTO FlashcardExercises (ExerciseId, Answer) VALUES 
(4, 'Tokyo');

-- Link Exercises to Quizzes
INSERT INTO QuizExercises (QuizId, ExerciseId) VALUES 
(1, 1),
(1, 2),
(2, 3),
(3, 4);

-- Insert Exam
INSERT INTO Exams (SectionId) VALUES (1);
DECLARE @examId INT = SCOPE_IDENTITY();

-- Link Exercises to Exam
INSERT INTO ExamExercises (ExamId, ExerciseId) VALUES 
(@examId, 1),
(@examId, 2),
(@examId, 3);

-- Update Users' last completed sections and quizzes
UPDATE Users SET LastCompletedSectionId = 1, LastCompletedQuizId = 1 WHERE Username = 'geo_learner1';
UPDATE Users SET LastCompletedSectionId = 2, LastCompletedQuizId = 2 WHERE Username = 'map_master';
