require "test_helper"

class CommentMailboxTest < ActionMailbox::TestCase
  test "register comment from reply" do
    id = Comment.last.id

    assert_difference("Comment.count") do
      receive_inbound_email_from_mail \
        charset: "UTF-8",
        to: "'blog_app' <comment+#{id}@example.com>",
        from: "'someone' <someone@example.com>",
        subject: "Re: notification",
        body: "コメントへの返信です"
    end

    assert_equal "コメントへの返信です", Comment.last.body
  end
end
