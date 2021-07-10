package commands

import "strings"

type CommandService struct {
	register map[string]*Command
}

func NewCommandService() CommandService {
	s := CommandService{
		register: make(map[string]*Command),
	}

	return s
}

func (s *CommandService) AddCommand(command *Command) {
	s.register[command.Name] = command
}

func (s *CommandService) ExecuteCommand(ctx *Context) {
	content := strings.Trim(ctx.Message.Content, "!")
	cmd := strings.Split(content, " ")[0]

	value, ok := s.register[cmd]
	if !ok {
		return
	}

	value.Executer(ctx)
}
