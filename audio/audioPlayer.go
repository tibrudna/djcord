package audio

type AudioPlayer struct {
	AudioSendHandler *AudioSendHandler
	interrupt        chan bool
}

func (p *AudioPlayer) Play(streamUrl string) {
	p.interrupt = make(chan bool, 1)
	p.AudioSendHandler.Send(streamUrl, p.interrupt)
}

func (p *AudioPlayer) Next() {
	p.interrupt <- true
}
